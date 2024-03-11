namespace ItaliaTreni.Api.Model.Response;

public sealed class GetAllFileResponse
{
    public List<FileResponse> Files { get; set; }

    public GetAllFileResponse()
    {
        Files = new List<FileResponse>();
    }
}
public sealed class FileResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime InsertDate { get; set; }
}