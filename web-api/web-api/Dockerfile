# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project files
COPY web-api.csproj ./
RUN dotnet restore

# Copy the rest of the application files
COPY . ./
RUN dotnet publish -c Release -o /out

# Use the official .NET runtime image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out .

# Install SQL Server
FROM mcr.microsoft.com/mssql/server:2022-latest AS sqlserver
WORKDIR /app

# Set environment variables for SQL Server
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=YourStrongPassword123
ENV MSSQL_PID=Express

# Expose the ports for the application and SQL Server
EXPOSE 5000
EXPOSE 1433

# Wait for SQL Server to initialize before starting the .NET application
COPY wait-for-sqlserver.sh /app/wait-for-sqlserver.sh
RUN chmod +x /app/wait-for-sqlserver.sh

# Start SQL Server and wait for it to initialize, then start the .NET app
CMD /bin/bash -c "/opt/mssql/bin/sqlservr & /app/wait-for-sqlserver.sh && dotnet web-api.dll"