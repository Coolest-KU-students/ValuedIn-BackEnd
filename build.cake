#addin nuget:?package=Cake.Docker&version=1.2.0

using Cake.Docker;
using Cake.Common.Diagnostics;

var target = Argument("target", listTasksTask);
var configuration = Argument("configuration", "Release");
var serverIsInDocker = Argument<bool>("dockerized", false);

string appDirectory = "./app";
string dockerComposeDirectory = "./";
string testingDirectory = "./tests";
string envFileName = "./.env";
string exampleAppEnvironment = ".env.example";
string integrationTestProject = testingDirectory + "/ValuedInBEIntegrationTests";
string unitTestProject = testingDirectory + "/ValuedInBEUnitTests";

string listTasksTask = "ListTasks";
string restoreNugetsTask = "Restore";
string initAppEnvironmentTask = "InitializeEnvironment";

string integrationTestTask = "TestIntegration";
string unitTestTask = "TestUnits";
string allTestTask = "Test";

string kafkaTask = "StartKafka";
string[] kafkaServices = { "zookeeper", "kafka", "init-kafka" };

string startDBTask = "StartDB";
string dockerizedDBContainerName = "sql-db";
string[] dbServices = { dockerizedDBContainerName };

string buildAndRunApplicationTask = "StartApp";
string[] buildServices = { "app" };

string stopAllTask = "Stop";

string[] taskArray = {initAppEnvironmentTask, listTasksTask, restoreNugetsTask, integrationTestTask, unitTestTask, allTestTask, kafkaTask, startDBTask, buildAndRunApplicationTask, stopAllTask};

target = taskArray.Contains(target) ? target : listTasksTask;

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
            string compiledTaskList = string.Join(", ", taskArray);
	        Information("Runnable tasks: " + compiledTaskList);
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
        DockerComposeUp(dockerComposeUpSettings, buildServices);
    });

Task(stopAllTask)
    .Does(() => {
        DockerComposeDown(dockerComposeDownSettings);
    });

RunTarget(target);