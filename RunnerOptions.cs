using Newtonsoft.Json;

namespace Slicer;

public class RunnerOptions
{
    [JsonProperty("initial-hap")]
    public string InitialHaps { get; set; }
    
    [JsonProperty("result-hap")]
    public string ResultHaps { get; set; }

    [JsonProperty("initial-legend")]
    public string InitialLegend { get; set; }

    [JsonProperty("result-legend")]
    public string ResultLegend { get; set; }

    [JsonProperty("start-position")]
    public int StartPosition { get; set; }

    [JsonProperty("end-position")]
    public int EndPosition { get; set; }

    [JsonProperty("sample-count")]
    public int SampleCount { get; set; }
}