public class DimRegion
{
    public Guid Id { get; set; }
    public string StateCode { get; set; }
    public string State { get; set; }
    public string Region { get; set; }
    public string ASGS_2016 { get; set; }
    public Guid PopulationId { get; set; }

    public FactPopulation Population { get; set; }
}