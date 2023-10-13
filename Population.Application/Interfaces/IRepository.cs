using System.Data;
using Population.Domain;

public interface IRepository
{
    Task LoadPopulationData(DataTable dataTable);
    Task NormalisePopulationTable();
    Task<List<SA4Population>> GetSA4SexPopulation(RegionCodeType regionCodeType, string genericRegionCode, string sex);
    Task<List<SA4PopulationAgeDiff>> GetSA4PopulationAgeDiff(RegionCodeType regionCodeType, string genericRegionCode, string sex, int yearLower, int yearHigher);
}