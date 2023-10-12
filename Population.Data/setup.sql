USE master;
GO
CREATE DATABASE PopulationDb;
GO
USE PopulationDb;
GO
CREATE SCHEMA PopulationSchema;
GO


CREATE TABLE [PopulationSchema].[TempLoad]
(
    [Id] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    [Sex_ABS] NVARCHAR(50),
    [Sex] NVARCHAR(50) NULL,
    [Age] NVARCHAR(50) NULL,
    [AgeString] NVARCHAR(50) NULL,
    [StateCode] NVARCHAR(50) NULL,
    [State] NVARCHAR(50) NULL,
    [REGIONTYPE] NVARCHAR(50) NULL,
    [Geography Level] NVARCHAR(50) NULL,
    [ASGS_2016] NVARCHAR(50) NULL,
    [Region] NVARCHAR(50) NULL,
    [PopulationTime] int NULL,
    [Census year] int NULL,
    [PopulationValue] int NULL,
    [Flag Codes] NVARCHAR(50) NULL,
    [Flags] NVARCHAR(50) NULL
)

CREATE TABLE [PopulationSchema].[FactPopulation]
(
    [Id] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    [PopulationTime] int NULL,
    [Census year] int NULL,
    [PopulationValue] int NULL,
)
GO

CREATE TABLE [PopulationSchema].[DimAge]
(
    [Id] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    [Age] NVARCHAR(50) NULL,
    [AgeString] NVARCHAR(50) NULL,
    [PopulationId] UNIQUEIDENTIFIER,
    CONSTRAINT [FK_DimAge_PopulationId] FOREIGN KEY ([PopulationId]) REFERENCES [PopulationSchema].[FactPopulation](Id)
)
GO

CREATE TABLE [PopulationSchema].[DimSex]
(
    [Id] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    [Sex_ABS] NVARCHAR(50),
    [Sex] NVARCHAR(50) NULL,
    [PopulationId] UNIQUEIDENTIFIER,
    CONSTRAINT [FK_DimSex_PopulationId] FOREIGN KEY ([PopulationId]) REFERENCES [PopulationSchema].[FactPopulation](Id)

)
GO

CREATE TABLE [PopulationSchema].[DimRegion]
(
    [Id] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    [StateCode] NVARCHAR(50) NULL,
    [State] NVARCHAR(50) NULL,
    [Region] NVARCHAR(50) NULL,
    [PopulationId] UNIQUEIDENTIFIER,
    CONSTRAINT [FK_DimRegion_PopulationId] FOREIGN KEY ([PopulationId]) REFERENCES [PopulationSchema].[FactPopulation](Id)
)
GO

CREATE PROCEDURE [PopulationSchema].[sp_normalisePopulationTable]
AS
BEGIN
    INSERT INTO [PopulationSchema].[FactPopulation] ([Id], [PopulationTime], [Census year], [PopulationValue])
    SELECT [Id], [PopulationTime], [Census year], [PopulationValue]
    FROM [PopulationSchema].[TempLoad];

    INSERT INTO [PopulationSchema].[DimAge] ([Age], [AgeString], [PopulationId])
    SELECT [Age], [AgeString], [Id]
    FROM [PopulationSchema].[TempLoad];

    INSERT INTO [PopulationSchema].[DimSex] ([Sex], [Sex_ABS], [PopulationId])
    SELECT [Sex], [Sex_ABS], [Id]
    FROM [PopulationSchema].[TempLoad];

    INSERT INTO [PopulationSchema].[DimRegion] ([StateCode], [State], [Region], [PopulationId])
    SELECT [StateCode], [State], [Region], [Id]
    FROM [PopulationSchema].[TempLoad];
END