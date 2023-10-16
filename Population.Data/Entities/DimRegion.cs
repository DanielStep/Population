public class DimRegion
{
    public Guid Id { get; set; }
    public string StateCode { get; set; }
    public string State { get; set; }
    public string Region { get; set; }
    public string ASGS_2016 { get; set; }

    public virtual ICollection<FactPopulation> Populations { get; set; }
}