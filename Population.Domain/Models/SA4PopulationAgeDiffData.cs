namespace Population.Domain;
public class SA4PopulationAgeDiffData
{
    public string AgeString { get; set; }
    public int PopulationLower { get; set; }
    public int PopulationUpper { get; set; }
    public int PopulationDiff => PopulationUpper - PopulationLower;
}