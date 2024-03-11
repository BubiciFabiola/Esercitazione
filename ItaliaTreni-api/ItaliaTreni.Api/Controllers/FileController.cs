using AutoMapper;
using FluentValidation.Results;
using ItaliaTreni.Api.Model.Request;
using ItaliaTreni.Api.Model.Response;
using ItaliaTreni.Api.Services;
using ItaliaTreni.Api.Validators;
using ItaliaTreni.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace ItaliaTreni.Api.Controllers;

[ApiController]
[Route("api/files")]
public sealed class FileController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILogger<FileController> _logger;
    private readonly ItaliaTreniDbContext _dbContext;

    public FileController(IMapper mapper, ItaliaTreniDbContext dbContext, ILogger<FileController> logger)
    {
        _mapper = mapper;
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Get all files
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <remarks>Returns all files.</remarks>
    /// <response code="200">Returns the file details.</response>
    [HttpGet]
    [ProducesResponseType(typeof(GetAllFileResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllFiles(CancellationToken cancellationToken)
    {
        var service = new FileService(_dbContext, _mapper);

        var response = await service.GetAllFiles(cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Get data analysis
    /// </summary>
    /// <param name="id">Id of file</param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <remarks>Returns all file data in the range StartMM and EndMM that are outside the threshold.</remarks>
    /// <response code="200">Returns the file details.</response>
    /// <response code="400">The request is not valid. See the response for details.</response>
    /// <response code="404">The specified file does not exist.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetDataAnalysisResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDataAnalysis([FromRoute] Guid id, [FromBody] GetDataAnalysisRequest request, CancellationToken cancellationToken)
    {
        var service = new FileService(_dbContext, _mapper);

        var validator = new GetDataAnalysisRequestValidator();
        ValidationResult resultValidator = validator.Validate(request);
        if (!resultValidator.IsValid)
        {
            List<string> errors = new List<string>();
            foreach (var error in resultValidator.Errors)
            {
                errors.Add(error.ErrorMessage);
            }
            return BadRequest(String.Join(", ", errors));
        }

        var requestForService = _mapper.Map<ItaliaTreni.Api.Services.Model.GetDataAnalysisRequest>(request);
        requestForService.Id = id;
        var response = await service.GetDataAnalysis(requestForService, cancellationToken);
        if (response == null)
        {
            return NotFound("There are no files with id " + id.ToString());
        }

        return Ok(response);

    }

    /// <summary>
    /// Create
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <remarks>Create the file and all dataFile presente, but if the file is already present only the missing dataFile is added.</remarks>
    /// <response code="200">Returns the id of file.</response>
    /// <response code="400">The request is not valid. See the response for details.</response>
    [HttpPost]
    [ProducesResponseType(typeof(CreateFileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateFileRequest request, CancellationToken cancellationToken)
    {
        var service = new FileService(_dbContext, _mapper);

        var validator = new CreateFileRequestValidator();
        ValidationResult resultValidator = validator.Validate(request);
        if (!resultValidator.IsValid)
        {
            List<string> errors = new List<string>();
            foreach (var error in resultValidator.Errors)
            {
                errors.Add(error.ErrorMessage);
            }
            return BadRequest(String.Join(", ", errors));
        }

        var response = await service.CreateFile(request, cancellationToken);

        return Ok(response);
    }
}
