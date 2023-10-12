using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

public class Repository : IRepository
{
    private readonly IConfiguration _config;
    public Repository(IConfiguration config)
    {
        _config = config;
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
                sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Geography Level", "Geography Level"));
                sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ASGS_2016", "ASGS_2016"));
                sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Region", "Region"));
                sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TIME", "PopulationTime"));
                sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Census year", "Census year"));
                sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Value", "PopulationValue"));
                sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Flag Codes", "Flag Codes"));
                sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Flags", "Flags"));

                sqlBulkCopy.DestinationTableName = "PopulationSchema.TempLoad";
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
            using (SqlCommand command = new SqlCommand("PopulationSchema.sp_normalisePopulationTable", con))
            {
                con.Open();
                command.CommandType = CommandType.StoredProcedure;
                await command.ExecuteNonQueryAsync();
                con.Close();
            }
        }
    }
}