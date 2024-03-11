namespace ItaliaTreni.Api.Model.Request;

public sealed class CreateFileRequest
{
    public string Name { get; set; }
    public List<DataFile> Datas { get; set; }
}

public sealed class DataFile
{
    public int MM { get; set; }
    public double P1 { get; set; }
    public double P2 { get; set; }
    public double P3 { get; set; }
    public double P4 { get; set; }
}