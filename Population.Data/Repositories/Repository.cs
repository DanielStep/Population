using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Population.Domain;

public class Repository : IRepository
{
    private readonly IConfiguration _config;
    private readonly PopulationDbContext _dbContext;
    public Repository(IConfiguration config, PopulationDbContext dbContext)
    {
        _config = config;
        _dbContext = dbContext;
    }
    public async Task LoadPopulationData(DataTable dataTable)
    {
        using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DbConnection")))
        {
            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
            {
                sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Sex_ABS", "Sex_ABS"));
                sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Sex", "Sex"));
                sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("AGE", "Age"));
                sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Age", "AgeString"));
                sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("STATE", "StateCode"));
                sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("State", "State"));
                sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("REGIONTYPE", "REGIONTYPE"));
                sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Geography Level", "GeographyLevel"));
                sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ASGS_2016", "ASGS_2016"));
                sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Region", "Region"));
                sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TIME", "PopulationTime"));
                sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Census year", "CensusYear"));
                sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Value", "PopulationValue"));
                sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Flag Codes", "FlagCodes"));
                sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Flags", "Flags"));

                sqlBulkCopy.DestinationTableName = "PopulationFlat";
                con.Open();

                await sqlBulkCopy.WriteToServerAsync(dataTable);
                con.Close();
            }
        }
    }

    public async Task NormalisePopulationTable()
    {
        using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DbConnection")))
        {
            using (SqlCommand command = new SqlCommand("sp_normalisePopulationTable", con))
            {
                con.Open();
                command.CommandType = CommandType.StoredProcedure;
                await command.ExecuteNonQueryAsync();
                con.Close();
            }
        }
    }

    public async Task<List<SA4Population>> GetSA4SexPopulation(RegionCodeType regionCodeType, string genericRegionCode, string sex)
    {
        return await _dbContext.SA4PopData.FromSql($@"DECLARE @RegionCodeType INT = {(int)regionCodeType} 

                                                    SELECT State, ASGS_2016, Region, Age, AgeString, Sex, PopulationValue, CensusYear FROM FactPopulation
                                                    JOIN DimRegion on FactPopulation.DimRegionFk = DimRegion.Id
                                                    JOIN DimAge on FactPopulation.DimAgeFk = DimAge.Id
                                                    JOIN DimSex on FactPopulation.DimSexFk = DimSex.Id
                                                    WHERE ((@RegionCodeType = 0 AND DimRegion.StateCode = {genericRegionCode})
                                                            OR 
                                                        (@RegionCodeType = 1 AND DimRegion.ASGS_2016 = {genericRegionCode}))
                                                        AND 
                                                        DimSex.Sex_ABS = {sex}
                                                    ORDER BY TRY_CONVERT(INT, DimRegion.ASGS_2016), TRY_CONVERT(INT, DimAge.Age), CensusYear;")
                                          .ToListAsync();
    }

    public async Task<List<SA4PopulationAgeDiff>> GetSA4PopulationAgeDiff(RegionCodeType regionCodeType, string genericRegionCode, string sex, int yearLower, int yearHigher)
    {
        return await _dbContext.SA4PopulationAgeDiffData.FromSql($@"DECLARE @RegionCodeType INT;
                                                                    SET @RegionCodeType = {(int)regionCodeType};

                                                                    WITH CensusLower AS (
                                                                        SELECT [State], Age, AgeString, Region, Sex, PopulationValue AS PopulationLower
                                                                        FROM FactPopulation
                                                                        JOIN DimRegion on FactPopulation.DimRegionFk = DimRegion.Id
                                                                        JOIN DimAge on FactPopulation.DimAgeFk = DimAge.Id
                                                                        JOIN DimSex on FactPopulation.DimSexFk = DimSex.Id
                                                                        WHERE CensusYear = {yearLower} AND
                                                                        ((@RegionCodeType = 0 AND DimRegion.StateCode = {genericRegionCode})
                                                                            OR 
                                                                        (@RegionCodeType = 1 AND DimRegion.ASGS_2016 = {genericRegionCode}))
                                                                        AND 
                                                                        Sex_ABS = {sex}
                                                                    ),
                                                                    CensusUpper AS (
                                                                        SELECT Age, Region, PopulationValue AS PopulationUpper
                                                                        FROM FactPopulation
                                                                        JOIN DimRegion on FactPopulation.DimRegionFk = DimRegion.Id
                                                                        JOIN DimAge on FactPopulation.DimAgeFk = DimAge.Id
                                                                        JOIN DimSex on FactPopulation.DimSexFk = DimSex.Id
                                                                        WHERE CensusYear = {yearHigher} AND
                                                                        ((@RegionCodeType = 0 AND DimRegion.StateCode = {genericRegionCode})
                                                                            OR 
                                                                        (@RegionCodeType = 1 AND DimRegion.ASGS_2016 = {genericRegionCode}))
                                                                        AND
                                                                        Sex_ABS = {sex}
                                                                    )

                                                                    SELECT 
                                                                        c1.AgeString,
                                                                        c1.Region,
                                                                        c1.Sex,
                                                                        c1.[State],
                                                                        c1.PopulationLower AS PopulationLower,
                                                                        c2.PopulationUpper AS PopulationUpper,
                                                                        (c2.PopulationUpper - c1.PopulationLower) AS PopulationDiff
                                                                    FROM CensusLower c1
                                                                    JOIN CensusUpper c2 on c1.Age = c2.Age AND c1.Region = c2.Region
                                                                    ORDER BY c1.Region, TRY_CONVERT(INT, c1.Age);
                                                                    ")
                                                                    .ToListAsync();
    }

}