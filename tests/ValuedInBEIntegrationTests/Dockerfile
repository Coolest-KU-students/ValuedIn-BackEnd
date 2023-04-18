# Use the official .NET Core 6 SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
# Copy the source code for the ValuedIn project
COPY /app /src/app

# Copy the source code for the ValuedInIntegrationTests project
COPY /tests/ValuedInBEUnitTests /src/tests/ValuedInBEUnitTests

# Copy the source code for the ValuedInIntegrationTests project
COPY /tests/ValuedInBEIntegrationTests /src/tests/ValuedInBEIntegrationTests

WORKDIR /src/tests/ValuedInBEIntegrationTests
# Run the tests
ENTRYPOINT ["dotnet", "test"]