using Population.Domain;

public class SA4PopulationDiffResult
{
    public string regionCode { get; set; }
    public string regionName { get; set; }
    public string censusYear { get; set; }
    public List<SA4PopulationDiffDTO> data { get; set; }
    public SA4PopulationDiffResult(RegionCodeType regionCodeType, string genericRegionCode, int yearLower, int yearHigher, List<SA4PopulationAgeDiff> data)
    {
        this.regionCode = genericRegionCode;
        this.regionName = regionCodeType == RegionCodeType.StateCode ? data.FirstOrDefault()?.State : data.FirstOrDefault()?.Region;
        this.censusYear = $"{yearLower}-{yearHigher}";

        var specificRegion = regionCodeType == RegionCodeType.StateCode ? data.FirstOrDefault()?.Region : null;
        this.data = MapDataToDTO(data,specificRegion);
    }

    private List<SA4PopulationDiffDTO> MapDataToDTO(List<SA4PopulationAgeDiff> data, string region)
    {
        var dtos = new List<SA4PopulationDiffDTO>();
        data.ForEach(x =>
        {
            dtos.Add(new SA4PopulationDiffDTO()
            {
                age = x.AgeString,
                population = x.PopulationDiff,
                sex = x.Sex,
                region = region
            });
        });
        return dtos;
    }
}