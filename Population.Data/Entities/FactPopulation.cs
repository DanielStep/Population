public class FactPopulation
{
    public Guid Id { get; set; }
    public int? PopulationTime { get; set; }
    public int? CensusYear { get; set; }
    public int? PopulationValue { get; set; }

    public DimAge Age { get; set; }
    public DimSex Sex { get; set; }
    public DimRegion Region { get; set; }
}