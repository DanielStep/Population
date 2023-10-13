using Newtonsoft.Json;

public class SA4PopulationDTO
{
    public string age { get; set; }
    public string sex { get; set; }
    public int censusYear { get; set; }
    public int population { get; set; }
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string region { get; set; }
}