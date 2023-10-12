using System.Data;
using Population.Domain;

public interface IRepository
{
    Task LoadPopulationData(DataTable dataTable);
    Task NormalisePopulationTable();
    Task<List<SA4PopulationData>> GetSA4PopData(string ASGS_2016, string sex);
}