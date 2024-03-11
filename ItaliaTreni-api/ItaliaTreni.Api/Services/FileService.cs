using AutoMapper;
using ItaliaTreni.Api.Model.Response;
using ItaliaTreni.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ItaliaTreni.Api.Services;

public sealed class FileService
{
    private readonly IMapper _mapper;
    private readonly ItaliaTreniDbContext _dbContext;

    public FileService(ItaliaTreniDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<GetAllFileResponse> GetAllFiles(CancellationToken cancellationToken)
    {
        var response = new GetAllFileResponse();
        try
        {
            var files = _dbContext.Files.ToList();
            foreach (var file in files)
            {
                response.Files.Add(_mapper.Map<FileResponse>(file));
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return response;
    }

    public async Task<GetDataAnalysisResponse> GetDataAnalysis(Api.Services.Model.GetDataAnalysisRequest request, CancellationToken cancellationToken)
    {
        var response = new GetDataAnalysisResponse();
        try
        {
            var file = await _dbContext.Files.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (file is null)
            {
                return null;
            }

            //Recovery of all data outside the threshold
            var dataFiles = file.FileDatas.Where(x => x.MM >= request.StartMM && x.MM <= request.EndMM &&
                                (x.P1 > request.Theshold || x.P2 > request.Theshold || x.P3 > request.Theshold || x.P4 > request.Theshold))
                                .ToList();

            response.Id = request.Id;
            response.Name = file.Name;
            response.DataFiles = new List<DataFileResponse>();
            foreach (var currentData in dataFiles)
            {
                response.DataFiles.Add(_mapper.Map<DataFileResponse>(currentData));
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return response;
    }

    public async Task<CreateFileResponse> CreateFile(Api.Model.Request.CreateFileRequest request, CancellationToken cancellationToken)
    {
        var response = new CreateFileResponse();

        _dbContext.Database.BeginTransaction();
        try
        {
            //If a file with that name is already present in db, then I proceed only with adding the data, 
            //otherwise I first insert the file entity and then the associated data
            var file = await _dbContext.Files.Include(v => v.FileDatas).FirstOrDefaultAsync(x => x.Name.Trim().Equals(request.Name), cancellationToken);
            if (file is null)
            {
                file = new Domain.Model.File(request.Name);
                await _dbContext.Files.AddAsync(file);
            }

            foreach (var currentData in request.Datas)
            {
                if (!file.FileDatas.Any(x => x.MM == currentData.MM))
                {
                    file.AddFileData(currentData.MM, currentData.P1,
                         currentData.P2, currentData.P3, currentData.P4);
                }
            }
            await _dbContext.SaveChangesAsync();
            _dbContext.Database.CommitTransaction();

            response.Id = file.Id;
        }
        catch (Exception ex)
        {
            _dbContext.Database.RollbackTransaction();
            throw new Exception(ex.Message);
        }

        return response;
    }
}
