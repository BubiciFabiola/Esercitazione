namespace ItaliaTreni.Api.Services.Model;

public sealed class GetDataAnalysisRequest
{
    public Guid Id { get; set; }
    public int StartMM { get; set; }
    public int EndMM { get; set; }
    public double Theshold { get; set; }
}
