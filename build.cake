#addin nuget:?package=Cake.Docker&version=1.2.0

using Cake.Docker;
using Cake.Common.Diagnostics;


List<string> taskList = new();
Dictionary<string, string> taskExplanations = new();
Dictionary<string, List<string>> nameAliases = new();


string appDirectory = "./app";
string dockerComposeDirectory = "./";
string testingDirectory = "./tests";
string envFileName = "./.env";
string exampleAppEnvironment = ".env.example";
string integrationTestProject = testingDirectory + "/ValuedInBEIntegrationTests";
string unitTestProject = testingDirectory + "/ValuedInBEUnitTests";

string listTasksTask = "ListTasks";
taskList.Add(listTasksTask);
taskExplanations.Add(listTasksTask, "Lists all tasks executable by Cake");
nameAliases.Add(listTasksTask, new List<string>{listTasksTask, "tasks", "list"});
string restoreNugetsTask = "Restore";
taskList.Add(restoreNugetsTask);
taskExplanations.Add(restoreNugetsTask, "Restores nugets in the solution");
nameAliases.Add(restoreNugetsTask, new List<string>{restoreNugetsTask, "restorenugets", "nugets"});
string initAppEnvironmentTask = "InitializeEnvironment";
taskList.Add(initAppEnvironmentTask);
taskExplanations.Add(initAppEnvironmentTask, "Initializes Environment variables");
nameAliases.Add(initAppEnvironmentTask, new List<string>{initAppEnvironmentTask, "env", "initenv"});

string integrationTestTask = "TestIntegration";
taskList.Add(integrationTestTask);
taskExplanations.Add(integrationTestTask, "Launch integration tests");
nameAliases.Add(integrationTestTask, new List<string>{integrationTestTask, "testi", "integrate"});
string unitTestTask = "TestUnits";
taskList.Add(unitTestTask);
taskExplanations.Add(unitTestTask, "Launch unit tests");
nameAliases.Add(unitTestTask, new List<string>{unitTestTask, "testu", "unit"});
string allTestTask = "Test";
taskList.Add(allTestTask);
taskExplanations.Add(allTestTask, "Launch unit tests and integration tests");
nameAliases.Add(allTestTask, new List<string>{allTestTask});

string kafkaTask = "StartKafka";
taskList.Add(kafkaTask);
taskExplanations.Add(kafkaTask, "Compose kafka container and its dependancies");
nameAliases.Add(kafkaTask, new List<string>{kafkaTask, "kafka"});
string[] kafkaServices = { "zookeeper", "kafka", "init-kafka" };

string startDBTask = "StartDB";
taskList.Add(startDBTask);
taskExplanations.Add(startDBTask, "Compose database container");
nameAliases.Add(startDBTask, new List<string>{startDBTask, "db", "database"});
string dockerizedDBContainerName = "sql-db";
string[] dbServices = { dockerizedDBContainerName };

string buildAndRunApplicationTask = "BuildApp";
taskList.Add(buildAndRunApplicationTask);
taskExplanations.Add(buildAndRunApplicationTask, "Builds the app into a docker container, and composes its dependencies");
nameAliases.Add(buildAndRunApplicationTask, new List<string>{buildAndRunApplicationTask, "app", "build"});
string[] buildAppServices = { "app" };

string stopAllTask = "Stop";
taskList.Add(stopAllTask);
taskExplanations.Add(stopAllTask, "Stops any active tasks and composes down their dependancies");
nameAliases.Add(stopAllTask, new List<string>{stopAllTask});

string[] taskArray = {initAppEnvironmentTask, listTasksTask, restoreNugetsTask, integrationTestTask, unitTestTask, allTestTask, kafkaTask, startDBTask, buildAndRunApplicationTask, stopAllTask};

var target = Argument("target", listTasksTask);
target = Argument("t", target); //shorthand
string targetTaskName = nameAliases.FirstOrDefault(pair => pair.Value.Contains(target)).Key;
target = taskArray.Contains(targetTaskName) ? targetTaskName : target;

// Define the settings for docker-compose up command
DockerComposeUpSettings dockerComposeUpSettings = new DockerComposeUpSettings {
    DetachedMode = true,
    ProjectDirectory = dockerComposeDirectory
};

// Define the settings for docker-compose down command
DockerComposeDownSettings dockerComposeDownSettings = new DockerComposeDownSettings {
    ProjectDirectory = dockerComposeDirectory
};

DockerComposeUpSettings integrationTestDockerComposeUpSettings = new DockerComposeUpSettings {
    ProjectName = integrationTestTask.ToLower(),
    DetachedMode = true,
    ProjectDirectory = integrationTestProject,
    ForceRecreate = true
};

DockerComposeDownSettings integrationTestDockerComposeDownSettings = new DockerComposeDownSettings {
    ProjectDirectory = integrationTestProject,
   // Rmi = "all",
    //Volumes = true
};

Task(listTasksTask)
    .Does((ctx)=>{
            Information("Existing tasks: ");
            foreach (string task in taskList)
            {
                string composedDescription = $"{task} - {taskExplanations[task]}. Aliases: {string.Join(", ", nameAliases[task])}";
                Information(composedDescription);
            }

            //string compiledTaskList = string.Join(", ", taskArray);
	       // Information("Runnable tasks: " + compiledTaskList);
        });

Task(restoreNugetsTask)
    .Does((ctx)=>{
            DotNetRestore(appDirectory);
        });

Task(initAppEnvironmentTask)
    .Does(()=>{
           Information("Initializing app environment...");
           Information($"Setting the DB Server connection for {dockerizedDBContainerName}");
           
            // Write the updated connection string back to the .env file
           string adjustedEnvironment = 
                System.IO.File.ReadAllText(exampleAppEnvironment)
                .Replace("{DB_Server}", dockerizedDBContainerName);

            System.IO.File.WriteAllText( $"{appDirectory}/{envFileName}", adjustedEnvironment);
            System.IO.File.WriteAllText( $"{envFileName}", adjustedEnvironment);
            System.IO.File.WriteAllText( $"{integrationTestProject}/{envFileName}", adjustedEnvironment);
            System.IO.File.WriteAllText( $"{unitTestProject}/{envFileName}", adjustedEnvironment);
        });

//Testing
Task(integrationTestTask)
    .IsDependentOn(initAppEnvironmentTask)
    .Does(() =>{
            DockerComposeUp(integrationTestDockerComposeUpSettings);
            Information("Waiting for testing environment to start up...");
            System.Threading.Tasks.Task.Delay(30000).Wait(); //TODO: we could wait until init-kafka stops running
            try{
                DotNetTest(integrationTestProject);
            }
            finally{
                DockerComposeDown(integrationTestDockerComposeDownSettings);
            }
        });

Task(unitTestTask)
    .Does(() =>{
            DotNetTest(unitTestProject);
        });

Task(allTestTask)
    .IsDependentOn(integrationTestTask)
    .IsDependentOn(unitTestTask);


Task(kafkaTask)
    .Does(() => {
        DockerComposeUp(dockerComposeUpSettings, kafkaServices);
    });

Task(startDBTask)
    .Does(() => {
        DockerComposeUp(dockerComposeUpSettings, dbServices);
    });

Task(buildAndRunApplicationTask)
    .IsDependentOn(kafkaTask)
    .IsDependentOn(startDBTask)
    .IsDependentOn(restoreNugetsTask)
    .Does(() => {
        DockerComposeUp(dockerComposeUpSettings, buildAppServices);
    });

Task(stopAllTask)
    .Does(() => {
        DockerComposeDown(dockerComposeDownSettings);
    });

RunTarget(target);