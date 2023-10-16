docker build -t population-image .

docker run -u root -d -p 1433:1433 -v /Users/mac/Desktop/Population/Population.Data/database:/var/opt/mssql/data --name population-container population-image

docker exec population-container /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P Password123! -i /usr/src/app/setup.sql

dotnet run Population.Importer/Population.Importer.csproj Population.Data/ABS_C16_T01_TS_SA_08062021164508583.xls

Limitations

Due to time constraints, a test database was not spun up as it ordinarily should. Dummy data was seeded and then torn down on the application database. Of course automated testing should never be done on the application database due to risk of data corruption.