namespace ItaliaTreni.Api.Model.Response;

public class GetDataAnalysisResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<DataFileResponse> DataFiles { get; set; }
}

public sealed class DataFileResponse
{
    public Guid Id { get; set; }
    public int MM { get; set; }
    public float P1 { get; set; }
    public float P2 { get; set; }
    public float P3 { get; set; }
    public float P4 { get; set; }
}
