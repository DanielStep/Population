public class DimSex
{
    public Guid Id { get; set; }
    public string Sex_ABS { get; set; }
    public string Sex { get; set; }
    public Guid PopulationId { get; set; }

    public FactPopulation Population { get; set; }
}