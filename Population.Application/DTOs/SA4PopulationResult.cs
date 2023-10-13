using Population.Domain;

public class SA4PopulationQueryResult
{
    public string regionCode { get; set; }
    public string regionName { get; set; }
    public List<SA4PopulationDTO> data { get; set; }
    public SA4PopulationQueryResult(RegionCodeType regionCodeType, string genericRegionCode, List<SA4Population> data)
    {
        this.regionCode = genericRegionCode;
        this.regionName = regionCodeType == RegionCodeType.StateCode ? data.FirstOrDefault()?.State : data.FirstOrDefault()?.Region;
        this.data = MapDataToDTO(data, regionCodeType);
    }

    private List<SA4PopulationDTO> MapDataToDTO(List<SA4Population> data, RegionCodeType regionCodeType)
    {
        var dtos = new List<SA4PopulationDTO>();
        data.ForEach(x =>
        {
            dtos.Add(new SA4PopulationDTO()
            {
                age = x.AgeString,
                censusYear = x.CensusYear,
                population = x.PopulationValue,
                sex = x.Sex,
                region = regionCodeType == RegionCodeType.StateCode ? x.Region : null
            });
        });
        return dtos;
    }
}