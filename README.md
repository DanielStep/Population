docker build -t population-image .

docker run -u root -d -p 1433:1433 -v /Users/mac/Desktop/Population/Population.Data/database:/var/opt/mssql/data --name population-container population-image

docker exec population-container /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P Password123! -i /usr/src/app/setup.sql