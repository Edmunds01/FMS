echo "Waiting for SQL Server to start..."
until /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrongPassword123 -Q "SELECT 1" > /dev/null 2>&1; do
    echo "Try to select"
    sleep 1
done
echo "SQL Server is up and running!"