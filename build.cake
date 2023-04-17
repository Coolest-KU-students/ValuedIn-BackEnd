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
            System.IO.File.WriteAllText( $"{integrationTestProject}/bin/Debug/net6.0/{envFileName}", adjustedEnvironment); //TODO: Fix workaround with Docker
            System.IO.File.WriteAllText( $"{unitTestProject}/{envFileName}", adjustedEnvironment);
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