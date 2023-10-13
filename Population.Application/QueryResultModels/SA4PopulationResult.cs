using Population.Domain;

public class SA4PopulationQueryResult
{
    public string regionCode { get; set; }
    public string regionName { get; set; }
    public List<SA4PopulationDTO> data { get; set; }
    public SA4PopulationQueryResult(string ASGS_2016, string regionName, List<SA4Population> data)
    {
        this.regionCode = ASGS_2016;
        this.regionName = regionName;
        this.data = MapDataToDTO(data);
    }

    private List<SA4PopulationDTO> MapDataToDTO(List<SA4Population> data)
    {
        var dtos = new List<SA4PopulationDTO>();
        data.ForEach(x =>
        {
            dtos.Add(new SA4PopulationDTO()
            {
                age = x.AgeString,
                censusYear = x.CensusYear,
                population = x.PopulationValue,
                sex = x.Sex
            });
        });
        return dtos;
    }
}