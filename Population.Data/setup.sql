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


CREATE TABLE [DimSex]
(
    [Id] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    [Sex_ABS] NVARCHAR(50) NOT NULL,
    [Sex] NVARCHAR(50) NOT NULL
);
GO

CREATE TABLE [DimAge]
(
    [Id] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    [Age] NVARCHAR(50) NOT NULL,
    [AgeString] NVARCHAR(50) NOT NULL
);
GO

CREATE TABLE [DimRegion]
(
    [Id] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    [StateCode] NVARCHAR(50) NOT NULL,
    [State] NVARCHAR(50) NOT NULL,
    [Region] NVARCHAR(50) NOT NULL,
    [ASGS_2016] NVARCHAR(50) NULL
);
GO

CREATE TABLE [FactPopulation]
(
    [Id] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    [PopulationTime] int NOT NULL,
    [CensusYear] int NOT NULL,
    [PopulationValue] int NOT NULL,
    [DimSexFk] UNIQUEIDENTIFIER NOT NULL,
    [DimAgeFk] UNIQUEIDENTIFIER NOT NULL,
    [DimRegionFk] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [FK_DimSex_Population] FOREIGN KEY ([DimSexFk]) REFERENCES [DimSex](Id),
    CONSTRAINT [FK_DimAge_Population] FOREIGN KEY ([DimAgeFk]) REFERENCES [DimAge](Id),
    CONSTRAINT [FK_DimRegion_Population] FOREIGN KEY ([DimRegionFk]) REFERENCES [DimRegion](Id)
);
GO

CREATE PROCEDURE sp_normalisePopulationTable
AS
BEGIN
    --Normalise Age dimension
    INSERT INTO DimAge (Age, AgeString)
    SELECT DISTINCT Age, AgeString 
    FROM PopulationFlat;

    --Normalise Region dimension
    INSERT INTO DimRegion (StateCode, [State], Region, ASGS_2016)
    SELECT DISTINCT 
        StateCode, 
        [State], 
        Region, 
        ASGS_2016
    FROM PopulationFlat;

    --Normalise Sex dimension
    INSERT INTO DimSex (Sex_ABS, Sex)
    SELECT DISTINCT 
        Sex_ABS, 
        Sex 
    FROM PopulationFlat;

    --Populate Fact table
    INSERT INTO FactPopulation (PopulationTime, CensusYear, PopulationValue, DimAgeFk, DimRegionFk, DimSexFk)
    SELECT 
        pf.PopulationTime, 
        pf.CensusYear, 
        pf.PopulationValue,
        da.Id AS DimAgeFk,
        dr.Id AS DimRegionFk,
        ds.Id AS DimSexFk
    FROM 
        PopulationFlat pf
    JOIN 
        DimAge da ON pf.Age = da.Age AND pf.AgeString = da.AgeString
    JOIN 
        DimRegion dr ON pf.StateCode = dr.StateCode AND pf.State = dr.State AND pf.Region = dr.Region AND pf.ASGS_2016 = dr.ASGS_2016
    JOIN 
        DimSex ds ON pf.Sex_ABS = ds.Sex_ABS AND pf.Sex = ds.Sex;
END