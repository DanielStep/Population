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

    public async Task<List<SA4PopulationData>> GetSA4PopData(string ASGS_2016, string sex)
    {
        return await _dbContext.SA4PopData.FromSql($@"SELECT ASGS_2016, Region, Age, AgeString, Sex, PopulationValue, CensusYear FROM FactPopulation
                                                        JOIN DimRegion on FactPopulation.Id = DimRegion.PopulationId
                                                        JOIN DimAge on FactPopulation.Id = DimAge.PopulationId
                                                        JOIN DimSex on FactPopulation.Id = DimSex.PopulationId
                                                        WHERE DimRegion.ASGS_2016 = {ASGS_2016} AND
                                                              DimSex.Sex_ABS = {sex}
                                                        ORDER BY TRY_CONVERT(INT, DimAge.Age), CensusYear;")
                                          .ToListAsync();
    }

}