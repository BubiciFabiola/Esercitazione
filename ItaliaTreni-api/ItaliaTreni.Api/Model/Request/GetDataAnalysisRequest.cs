namespace ItaliaTreni.Api.Model.Request;

public sealed class GetDataAnalysisRequest
{
    public int StartMM { get; set; }
    public int EndMM { get; set; }
    public float Theshold { get; set; }
}
