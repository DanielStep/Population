using System.Data;
using Population.Domain;

public interface IRepository
{
    Task LoadPopulationData(DataTable dataTable);
    Task NormalisePopulationTable();
    Task<List<SA4Population>> GetSA4SexPopulation(string ASGS_2016, string sex);
    Task<List<SA4PopulationAgeDiff>> GetSA4PopulationAgeDiff(string ASGS_2016, string sex, int yearLower, int yearHigher);
}