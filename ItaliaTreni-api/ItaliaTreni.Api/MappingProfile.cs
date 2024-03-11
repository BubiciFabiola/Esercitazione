namespace ItaliaTreni.Api;

public sealed class MappingProfile: AutoMapper.Profile
{
    public MappingProfile()
    {
        CreateMap<ItaliaTreni.Api.Model.Request.CreateFileRequest, ItaliaTreni.Api.Services.Model.GetDataAnalysisRequest>();
        CreateMap<ItaliaTreni.Domain.Model.File, ItaliaTreni.Api.Model.Response.FileResponse>();
        CreateMap<ItaliaTreni.Domain.Model.FileData, ItaliaTreni.Api.Model.Response.DataFileResponse>();
    }
}
