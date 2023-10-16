public class FactPopulation
{
    public Guid Id { get; set; }
    public int PopulationTime { get; set; }
    public int CensusYear { get; set; }
    public int PopulationValue { get; set; }

    public Guid DimSexFk { get; set; }
    public virtual DimSex DimSex { get; set; }

    public Guid DimAgeFk { get; set; }
    public virtual DimAge DimAge { get; set; }

    public Guid DimRegionFk { get; set; }
    public virtual DimRegion DimRegion { get; set; }
}