namespace F1Desktop.Models.ErgastAPI;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

public class MRData
{
    public string xmlns { get; set; }
    public string series { get; set; }
    public string url { get; set; }
    public string limit { get; set; }
    public string offset { get; set; }
    public string total { get; set; }
    public RaceTable RaceTable { get; set; }
}