# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project files
COPY web-api.csproj ./
RUN dotnet restore

# Copy the rest of the application files
COPY . ./
RUN dotnet publish -c Release -o /out

# Use the SQL Server image as the base for the runtime
FROM mcr.microsoft.com/mssql/server:2022-latest AS runtime
WORKDIR /app

# Install the .NET runtime
RUN apt-get update && \
    apt-get install -y wget && \
    wget https://dot.net/v1/dotnet-install.sh && \
    chmod +x dotnet-install.sh && \
    ./dotnet-install.sh --channel 8.0 --runtime aspnetcore && \
    rm dotnet-install.sh && \
    apt-get clean

# Set environment variables for SQL Server
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=YourStrongPassword123
ENV MSSQL_PID=Express

# Copy the published .NET application
COPY --from=build /out .

# Expose the ports for the application and SQL Server
EXPOSE 5000
EXPOSE 1433

# Copy the wait-for-sqlserver.sh script
COPY wait-for-sqlserver.sh /app/wait-for-sqlserver.sh

# Switch to root to modify permissions
USER root
RUN chmod +x /app/wait-for-sqlserver.sh

# Switch back to the default user
USER mssql

# Start SQL Server and wait for it to initialize, then start the .NET app
CMD /bin/bash -c "/opt/mssql/bin/sqlservr & /app/wait-for-sqlserver.sh && dotnet web-api.dll"