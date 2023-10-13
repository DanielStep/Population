using Population.Domain;

public class SA4PopulationDiffResult
{
    public string regionCode { get; set; }
    public string regionName { get; set; }
    public string censusYear { get; set; }
    public List<SA4PopulationDiffDTO> data { get; set; }
    public SA4PopulationDiffResult(string ASGS_2016, string regionName, int yearLower, int yearHigher, List<SA4PopulationAgeDiff> data)
    {
        this.regionCode = ASGS_2016;
        this.regionName = regionName;
        this.censusYear = $"{yearLower}-{yearHigher}";
        this.data = MapDataToDTO(data);
    }

    private List<SA4PopulationDiffDTO> MapDataToDTO(List<SA4PopulationAgeDiff> data)
    {
        var dtos = new List<SA4PopulationDiffDTO>();
        data.ForEach(x =>
        {
            dtos.Add(new SA4PopulationDiffDTO()
            {
                age = x.AgeString,
                population = x.PopulationDiff,
                sex = x.Sex
            });
        });
        return dtos;
    }
}