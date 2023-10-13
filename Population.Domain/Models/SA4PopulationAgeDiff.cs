namespace Population.Domain;
public class SA4PopulationAgeDiff
{
    public string AgeString { get; set; }
    public string Region { get; set; }
    public string Sex { get; set; }
    public int PopulationLower { get; set; }
    public int PopulationUpper { get; set; }
    public int PopulationDiff => PopulationUpper - PopulationLower;
}