#addin nuget:?package=Cake.Docker&version=1.2.0

using Cake.Docker;
using Cake.Common.Diagnostics;

var target = Argument("target", listTasksTask);
var configuration = Argument("configuration", "Release");
var DBServer = Argument("server", "localhost");
var serverIsInDocker = Argument<bool>("dockerized", false);

string appDirectory = "./app";
string dockerComposeDirectory = "./";
string testingDirectory = "./tests";
string envFile = "./.env";
string exampleAppEnvironment = ".env.app";
string integrationTestProject = testingDirectory + "/ValuedInBEIntegrationTests/ValuedInBEIntegrationTests.csproj";
string unitTestProject = testingDirectory + "/ValuedInBEUnitTests/ValuedInBEUnitTests.csproj";

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
           
           DBServer = serverIsInDocker ? dockerizedDBContainerName : DBServer;
           Information($"Setting the DB Server connection for {DBServer}");

            // Write the updated connection string back to the .env file
            System.IO.File.WriteAllText(
                $"{appDirectory}/{envFile}",
                System.IO.File.ReadAllText(exampleAppEnvironment)
                               .Replace("{DB_Server}", DBServer)
            );

        });

//Testing
Task(integrationTestTask)
    .IsDependentOn(kafkaTask)
    .IsDependentOn(initAppEnvironmentTask)
    .Does(() =>{
            DotNetTest(integrationTestProject);
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
    .IsDependentOn(allTestTask)
    .IsDependentOn(restoreNugetsTask)
    .Does(() => {
        DockerComposeUp(dockerComposeUpSettings, buildServices);
    });

Task(stopAllTask)
    .Does(() => {
        DockerComposeDown(dockerComposeDownSettings);
    });

RunTarget(target);