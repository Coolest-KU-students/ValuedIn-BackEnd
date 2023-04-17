# ValuedIn .NET Backend Service

## Documentation

Service documentation is hosted [here](https://github.com/Coolest-KU-students/ValuedIn-Documentation)

This repo uses a Cake configuration file for building and managing a .NET backend service using Docker and Docker Compose.

### Components:
- **Kafka**: A distributed streaming platform that enables building real-time data streaming applications.
- **MSSQL**: A relational database management system that provides a robust and scalable solution for storing and managing data.
- **Docker**: A containerization platform that allows for easy deployment and management of applications in lightweight, portable containers.
- **Cake**: A build automation tool for .NET applications that allows for writing build scripts using C# or PowerShell.
- **xUnit**: A popular testing framework for .NET applications that provides a simple and extensible way to write unit tests.


## Prerequisites

Before running the tasks with Cake configuration, make sure you have the following prerequisites:

- Docker installed on your machine
- Docker Compose installed on your machine
- .NET 6.0 SDK installed on your machine

### Additional Information

- Everything can be launched via Cake or Visual Studio integration with Docker Compose, however environment has to be initiated either way.
- By default, the app uses the dockerized MSSQL, and the user should change environment variables if they need to change that.
- Users can ignore the 'init-kafka' container as the only purpose it serves is to initialize data in Kafka.


## Quickstart

To quickly get started with building and running the .NET backend service, follow the steps below:

1. Initialize default environment variables with this commnd:

    ```sh
    dotnet cake --target=InitializeEnvironment
    ```

2. You can use VS to attach to the default Docker Compose file, or alternatively, Launch the service by running the following command:

    ```sh
    dotnet cake --target=BuildAndRunApplication
    ```

    This will build and run the application along with the Kafka and database services in detached mode using the same Docker Compose.


After completing these steps, you should have the .NET backend service up and running, along with Kafka and the database services started.

Note: Make sure to update the paths and settings in the Cake configuration to match your specific environment and requirements.

## Detailed Steps

Follow the steps below to initialize the environment, run tests, and start the application:

### Tasks

This Cake configuration includes the following tasks:

- `RestoreNugets`: Restores NuGet packages for the application.
- `StartKafka`: Starts the Kafka and database services.
- `IntegrationTest`: Runs integration tests.
- `UnitTest`: Runs unit tests.
- `AllTest`: Runs both integration and unit tests together.
- `BuildAndRunApplication`: Builds and runs the application along with the Kafka and database services.
- `StopAll`: Stops all Docker services.

### Running the Tasks

1. Clone the repository and navigate to the root directory of the project.

2. Ensure that Docker is running on your machine.

3. Open a terminal or command prompt and run the following command to restore NuGet packages for the application:

    ```sh
    dotnet cake --target=Restore
    ```

4. Next, start the Kafka and database services using the following command:

    ```sh
    dotnet cake --target=StartKafka
    ```

5. After starting Kafka, run the following command to run unit tests:

    ```sh
    dotnet cake --target=UnitTest
    ```

6. Then, run the following command to run integration tests:

    ```sh
    dotnet cake --target=IntegrationTest
    ```

7. If you want to run both integration and unit tests together, you can use the following command:

    ```sh
    dotnet cake --target=AllTest
    ```

8. After running the tests, you can build and run the application along with the Kafka and database services using the following command:

    ```sh
    dotnet cake --target=BuildAndRunApplication
    ```

    This will build and run the application in detached mode using Docker Compose.

9. To stop all the Docker services, you can use the following command:

    ```sh
    dotnet cake --target=StopAll
    ```

Some tasks rerun others due to dependancy tree. If You want to attempt to launch explicit tasks, add ```--exclusive``` flag

Note: Make sure to update the paths and settings in the Cake configuration to match your specific environment and requirements.

Please let me know if you need further assistance!