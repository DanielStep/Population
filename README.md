### Usage

Requirements:

Docker Desktop
.NET 6 SDK

Database 

The database run in a docker container and requires setup before the Population.Importer or Api application can be run.

Run the following command from solution root directory:

`docker build -t population-image Population.Data/`

Docker requires full path to the Population folder:

`docker run -u root -d -p 1433:1433 -v [full path to Population.Data folder]/database:/var/opt/mssql/data --name population-container population-image`

eg.

`docker run -u root -d -p 1433:1433 -v /Users/mac/Desktop/Population/Population.Data/database:/var/opt/mssql/data --name population-container population-image`


`docker exec population-container /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P Password123! -i /usr/src/app/setup.sql`

Database tables, stored procedures and views should now be set up an available on localhost:1433

The following connection string may be used:

`Server=localhost,1433;Database=Population;User Id=sa;Password=Password123!;`

Population.Importer

From the Population.Importer directory run the following command:

`dotnet run ../Population.Data/ABS_C16_T01_TS_SA_08062021164508583.xls1`

Population.Api

From the Population.Api directory run the following command:

`dotnet run` 

Using any HTTP client, make a GET request to the following endpoints:

`http://localhost:5000/api/age-structure/102/1`

`http://localhost:5000/api/age-structure-diff/1/1/2011/2016`

Tests

Please ensure that the database is set up and running prior to running to Integration Tests.

### Architecture

Importer

For expediency and due to bulk nature of the csv import, a simple console application was chose for this. It is very thin and allows shared Application layer to perform the operation by notifying Mediator of import Command to be picked up by a handler.

The Importer could be separated out as a separate service.

API Layer

This is standard ASP.NET Web API. The controllers are thin and only handle routing and serialization. A Mediator is notified of a particular  query request and the appropriate handler picks it up.

Application Layer

The application layer follows CQRS pattern and Mediator patterns for separation of concerns. Queries used the Api and Commands used by the Population.Importer are easy separated by respective handlers.

Domain Layer

While there is a Domain layer present to hold immediate results of queries, it is not strictly needed for the given requirements. It would ideal for business logic like calculation of age difference to be done in the domain layer, but for performance reasons this was done in the database.

Data Layer

The Data layer holds the database context as well as a Repository that that abstracts it away from the application. This makes mock and testing easier. Some operations like test data creation are modeled by Entity Framework, while others are bulk inserts handled directly eg SqlBulkCopy.

vw_PopulationBy5YearIntervals View has been created in the database. This was written to a certain level of granularity, but can be modified by removing groupings.

### Limitations

Due to time constraints, the following limitations exist:

- Connection string are hardcoded into appsettings.json containing secrets. Of course these should be injected at during deployment from secrets vault.

- There is insufficient validation of the csv data being imported. Given more time validation would be done on the data to ensure it is in the correct format, and also to avoid duplicate data eg for existing Census Years.

- SQL queries are hardcoded into the repository. Given more time these would be moved to stored procedures.

- Unit tests should be written to cover the repository and service layers. As well as more detailed integration tests, the types of scenarios can be discussed further.

- Integration test are run on application database. Of course automated testing should never be done on the application database due to risk of data corruption, and a test database should be reproduced. This was mitigated by seeding and tearing down of isolated test data.

- Ordinary business logic such as calculation of the age difference would be done in domain models. However due to performance considerations, this was done in the database.

