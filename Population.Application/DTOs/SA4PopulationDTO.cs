using Population.Domain;

public class SA4PopulationDataQueryResult
{
    public SA4PopulationDataQueryResult(string ASGS_2016, string regionName, List<SA4PopulationData> data)
    {
        this.regionCode = ASGS_2016;
        this.regionName = regionName;
        this.data = MapDataToDTO(data);
    }

    public string regionCode { get; set; }
    public string regionName { get; set; }
    public List<SA4PopulationDataDTO> data { get; set; }

    private List<SA4PopulationDataDTO> MapDataToDTO(List<SA4PopulationData> data)
    {
        var dtos = new List<SA4PopulationDataDTO>();
        data.ForEach(x =>
        {
            dtos.Add(new SA4PopulationDataDTO()
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

public class SA4PopulationDataDTO
{
    public string age { get; set; }
    public string sex { get; set; }
    public int censusYear { get; set; }
    public int population { get; set; }
}