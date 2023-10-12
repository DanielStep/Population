USE master;
GO
CREATE DATABASE PopulationDb;
GO
USE PopulationDb;
GO


CREATE TABLE [PopulationFlat]
(
    [Id] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    [Sex_ABS] NVARCHAR(50),
    [Sex] NVARCHAR(50) NULL,
    [Age] NVARCHAR(50) NULL,
    [AgeString] NVARCHAR(50) NULL,
    [StateCode] NVARCHAR(50) NULL,
    [State] NVARCHAR(50) NULL,
    [REGIONTYPE] NVARCHAR(50) NULL,
    [GeographyLevel] NVARCHAR(50) NULL,
    [ASGS_2016] NVARCHAR(50) NULL,
    [Region] NVARCHAR(50) NULL,
    [PopulationTime] int NULL,
    [CensusYear] int NULL,
    [PopulationValue] int NULL,
    [FlagCodes] NVARCHAR(50) NULL,
    [Flags] NVARCHAR(50) NULL
)

CREATE TABLE [FactPopulation]
(
    [Id] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    [PopulationTime] int NULL,
    [CensusYear] int NULL,
    [PopulationValue] int NULL,
)
GO

CREATE TABLE [DimAge]
(
    [Id] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    [Age] NVARCHAR(50) NULL,
    [AgeString] NVARCHAR(50) NULL,
    [PopulationId] UNIQUEIDENTIFIER,
    CONSTRAINT [FK_DimAge_PopulationId] FOREIGN KEY ([PopulationId]) REFERENCES [FactPopulation](Id)
)
GO

CREATE TABLE [DimSex]
(
    [Id] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    [Sex_ABS] NVARCHAR(50),
    [Sex] NVARCHAR(50) NULL,
    [PopulationId] UNIQUEIDENTIFIER,
    CONSTRAINT [FK_DimSex_PopulationId] FOREIGN KEY ([PopulationId]) REFERENCES [FactPopulation](Id)

)
GO

CREATE TABLE [DimRegion]
(
    [Id] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    [StateCode] NVARCHAR(50) NULL,
    [State] NVARCHAR(50) NULL,
    [Region] NVARCHAR(50) NULL,
    [ASGS_2016] NVARCHAR(50) NULL,
    [PopulationId] UNIQUEIDENTIFIER,
    CONSTRAINT [FK_DimRegion_PopulationId] FOREIGN KEY ([PopulationId]) REFERENCES [FactPopulation](Id)
)
GO

CREATE PROCEDURE sp_normalisePopulationTable
AS
BEGIN
    INSERT INTO FactPopulation (Id, PopulationTime, [CensusYear], PopulationValue)
    SELECT Id, PopulationTime, [CensusYear], PopulationValue
    FROM PopulationFlat;

    INSERT INTO DimAge (Age, AgeString, PopulationId)
    SELECT Age, AgeString, Id
    FROM PopulationFlat;

    INSERT INTO DimSex (Sex, Sex_ABS, PopulationId)
    SELECT Sex, Sex_ABS, Id
    FROM PopulationFlat;

    INSERT INTO DimRegion (StateCode, [State], Region, ASGS_2016, PopulationId)
    SELECT StateCode, [State], Region, ASGS_2016, Id
    FROM PopulationFlat;
END