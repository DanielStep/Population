using System.Data;

public interface IRepository
{
    Task LoadPopulationData(DataTable dataTable);
    Task NormalisePopulationTable();
}