public class DimSex
{
    public Guid Id { get; set; }
    public string Sex_ABS { get; set; }
    public string Sex { get; set; }

    public virtual ICollection<FactPopulation> Populations { get; set; }
}
