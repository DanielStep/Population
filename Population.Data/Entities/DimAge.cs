public class DimAge
{
    public Guid Id { get; set; }
    public string Age { get; set; }
    public string AgeString { get; set; }
    public Guid PopulationId { get; set; }

    public FactPopulation Population { get; set; }
}