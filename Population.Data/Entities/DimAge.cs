public class DimAge
{
    public Guid Id { get; set; }
    public string Age { get; set; }
    public string AgeString { get; set; }

    public virtual ICollection<FactPopulation> Populations { get; set; }
}