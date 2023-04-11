#addin nuget:?package=Cake.Docker&version=1.2.0

using Cake.Docker;
using Cake.Common.Diagnostics;

string listTasksTask = "ListTasks";

var target = Argument("target", listTasksTask);
var configuration = Argument("configuration", "Release");

string appDirectory = "./app";

string restoreNugetsTask = "Restore";

string testingDirectory = "./tests";
string integrationTestTask = "TestIntegration";
string integrationTestProject = testingDirectory + "/ValuedInBEIntegrationTests/ValuedInBEIntegrationTests.csproj";
string unitTestTask = "TestUnits";
string unitTestProject = testingDirectory + "/ValuedInBEUnitTests/ValuedInBEUnitTests.csproj";
string allTestTask = "Test";

string kafkaTask = "StartKafka";
string[] kafkaServices = { "zookeeper", "kafka", "init-kafka" };

string startDBTask = "StartDB";
string[] dbServices = { "init-db" };

string buildAndRunApplicationTask = "StartApp";
string[] buildServices = { "app" };

string stopAllTask = "Stop";

string[] taskArray = {listTasksTask, restoreNugetsTask, integrationTestTask, unitTestTask, allTestTask, kafkaTask, startDBTask, buildAndRunApplicationTask, stopAllTask};

target = taskArray.Contains(target) ? target : listTasksTask;

// Define the settings for docker-compose up command
DockerComposeUpSettings dockerComposeUpSettings = new DockerComposeUpSettings {
    DetachedMode = true,
    ProjectDirectory = appDirectory
};

// Define the settings for docker-compose down command
DockerComposeDownSettings dockerComposeDownSettings = new DockerComposeDownSettings {
    ProjectDirectory = appDirectory
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

//Testing
Task(integrationTestTask)
    .IsDependentOn(kafkaTask)
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

// Define the Cake tasks
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