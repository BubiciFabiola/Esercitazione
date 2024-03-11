using ItaliaTreni.Domain.Model;
using ItaliaTreni.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace ItaliaTreni.Api.Tests.Services;

public class FileServiceTests : ItaliaTreniTestBase
{

    private Domain.Model.File file = null!;
    private Api.Services.FileService fileService = null!;

    public FileServiceTests()
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        fileService = new Api.Services.FileService(italiaTreniDbContext, mapper);
    }

    private async Task CreateStandarFile(string nameOfFile)
    {
        file = new Domain.Model.File(nameOfFile);
        italiaTreniDbContext.Files.Add(file);
        file.AddFileData(1, -7.053467, -7.0436, -7.03426, -7.02968);
        file.AddFileData(2, -4.5561523, 0, 0, 0);
        file.AddFileData(3, -4.6813965, 0, 0, 0);
        await italiaTreniDbContext.SaveChangesAsync();
    }

    #region GetAllFiles

    /// <summary>
    /// The database has no files.
    /// </summary>
    /// <returns>Returns an empty list of items. </returns>
    [Fact]
    public async Task GetAllFiles_ShouldReturnSuccess_WhitNoElements()
    {
        // Act
        var result = await fileService.GetAllFiles(default);

        // Assert
        Assert.Empty(result.Files);
    }

    /// <summary>
    /// In the database there are three files with related data.
    /// </summary>
    /// <returns>Returns a list containing the three files. Each item in the list contains the file information. </returns>
    [Fact]
    public async Task GetAllFiles_ShouldReturnSuccess_WhitElements()
    {
        // Arrange
        CreateStandarFile("file 1");
        CreateStandarFile("file 2");
        CreateStandarFile("file 3");

        // Act
        var result = await fileService.GetAllFiles(default);

        // Assert
        Assert.NotEmpty(result.Files);
        Assert.Equal(3, result.Files.Count);
    }

    #endregion

    #region GetDataAnalysis

    /// <summary>
    /// There is a file with related data in the database, but ask for data analysis from a file that is not present.
    /// </summary>
    /// <returns>Return null. </returns>
    [Fact]
    public async Task GetDataAnalysis_ShouldReturnNull_WhenTheFileRequestIsNotPresentInDb()
    {
        // Arrange
        CreateStandarFile("file 1");
        var request = new Api.Services.Model.GetDataAnalysisRequest
        {
            Id = new Guid(),
            StartMM = 1,
            EndMM = 3,
            Theshold = -6.01
        };

        // Act
        var result = await fileService.GetDataAnalysis(request, default);

        // Assert
        Assert.Null(result);
    }

    /// <summary>
    /// There is a file with related data in the database. You ask for data analysis of the file, but there is no data that is outside the threshold. 
    /// </summary>
    /// <returns>Returns the response where the file id and name are present, but the data list is empty.</returns>
    [Fact]
    public async Task GetDataAnalysis_ShouldReturnSuccess_WhitNoElements()
    {
        // Arrange
        CreateStandarFile("file 1");
        var request = new Api.Services.Model.GetDataAnalysisRequest
        {
            Id = file.Id,
            StartMM = 1,
            EndMM = 3,
            Theshold = 1
        };

        // Act
        var result = await fileService.GetDataAnalysis(request, default);

        // Assert
        Assert.Equal(file.Id, result.Id);
        Assert.Equal(file.Name, result.Name);
        Assert.Empty(result.DataFiles);
    }

    /// <summary>
    /// There is a file with related data in the database. You ask for data analysis of the file and three datas are outside the threshold.
    /// </summary>
    /// <returns>Returns the response where the file id and name are present and the data list have three element.</returns>
    [Fact]
    public async Task GetDataAnalysis_ShouldReturnSuccess_WhitElements()
    {
        // Arrange
        CreateStandarFile("file 1");
        var request = new Api.Services.Model.GetDataAnalysisRequest
        {
            Id = file.Id,
            StartMM = 1,
            EndMM = 3,
            Theshold = -8.01
        };

        // Act
        var result = await fileService.GetDataAnalysis(request, default);

        // Assert
        Assert.Equal(file.Id, result.Id);
        Assert.Equal(file.Name, result.Name);
        Assert.Equal(3, result.DataFiles.Count);
    }

    /// <summary>
    /// There is a file with related data in the database. You ask for data analysis of the file and part of datas are outside the threshold.
    /// </summary>
    /// <returns>Returns the response where the file id and name are present and the data list have elements that are outside the threshold.</returns>
    [Fact]
    public async Task GetDataAnalysis_ShouldReturnSuccess_WhitPartOfElements()
    {
        // Arrange
        CreateStandarFile("file 1");
        var request = new Api.Services.Model.GetDataAnalysisRequest
        {
            Id = file.Id,
            StartMM = 1,
            EndMM = 3,
            Theshold = -6.1
        };

        // Act
        var result = await fileService.GetDataAnalysis(request, default);

        // Assert
        Assert.Equal(file.Id, result.Id);
        Assert.Equal(file.Name, result.Name);
        Assert.Equal(2, result.DataFiles.Count);
    }

    #endregion

    #region CreateFile

    /// <summary>
    /// A file that is not present but has no data is inserted into the database.
    /// </summary>
    /// <returns>Return id of file. </returns>
    [Fact]
    public async Task CreateFile_ShouldReturnSuccess_WhenInsertOnlyFile()
    {
        // Arrange
        var request = new Api.Model.Request.CreateFileRequest
        {
            Name = "inserimento solo file",
            Datas = new List<Model.Request.DataFile>()
        };

        // Act
        var result = await fileService.CreateFile(request, default);

        // Assert
        var fileOfDb = italiaTreniDbContext.Files.First(x => x.Name.Equals(request.Name));
        Assert.Equal(request.Name, fileOfDb.Name);
        Assert.Equal(fileOfDb.Id, result.Id);
        Assert.Empty(fileOfDb.FileDatas);
    }

    /// <summary>
    /// A missing file with two records is inserted into the database.
    /// </summary>
    /// <returns>Return id of file. </returns>
    [Fact]
    public async Task CreateFile_ShouldReturnSuccess_WhenInsertFileAndDataFile()
    {
        // Arrange
        var request = new Api.Model.Request.CreateFileRequest
        {
            Name = "inserimento solo file",
            Datas = new List<Model.Request.DataFile>()
            {
                new Model.Request.DataFile(){MM = 1, P1=-5.053467, P2=0, P3=0, P4=0},
                new Model.Request.DataFile(){MM = 2, P1=-4.5561523, P2=0, P3=0, P4=0}
            }
        };

        // Act
        var result = await fileService.CreateFile(request, default);

        // Assert
        var fileOfDb = italiaTreniDbContext.Files.First(x => x.Name.Equals(request.Name));
        Assert.Equal(request.Name, fileOfDb.Name);
        Assert.Equal(fileOfDb.Id, result.Id);
        Assert.Equal(request.Datas.Count, fileOfDb.FileDatas.Count);
    }

    /// <summary>
    /// The file is present in the database, but without records. We proceed by inserting the records for that file.
    /// </summary>
    /// <returns>Return id of file. </returns>
    [Fact]
    public async Task CreateFile_ShouldReturnSuccess_WhenInsertOnlyDataFile()
    {
        // Arrange
        file = new Domain.Model.File("inserimento solo dati");
        await italiaTreniDbContext.SaveChangesAsync();
        var request = new Api.Model.Request.CreateFileRequest
        {
            Name = "inserimento solo dati",
            Datas = new List<Model.Request.DataFile>()
            {
                new Model.Request.DataFile(){MM = 1, P1=-5.053467, P2=0, P3=0, P4=0},
                new Model.Request.DataFile(){MM = 2, P1=-4.5561523, P2=0, P3=0, P4=0}
            }
        };

        // Act
        var result = await fileService.CreateFile(request, default);

        // Assert
        var fileOfDb = italiaTreniDbContext.Files.First(x => x.Name.Equals(request.Name));
        Assert.Equal(request.Name, fileOfDb.Name);
        Assert.Equal(fileOfDb.Id, result.Id);
        Assert.Equal(request.Datas.Count, fileOfDb.FileDatas.Count);
    }

    /// <summary>
    /// The file is present in the database, but with records. This is done by inserting new records for that file.
    /// </summary>
    /// <returns>Return id of file. </returns>
    [Fact]
    public async Task CreateFile_ShouldReturnSuccess_WhenThereAreDataFilesButRequestContainNewDataFile()
    {
        // Arrange
        CreateStandarFile("inserimento solo dati");
        var request = new Api.Model.Request.CreateFileRequest
        {
            Name = "inserimento solo dati",
            Datas = new List<Model.Request.DataFile>()
            {
                new Model.Request.DataFile(){MM = 4, P1=-4.4924316, P2=0, P3=0, P4=0},
                new Model.Request.DataFile(){MM = 5, P1=-4.251709, P2=0, P3=0, P4=0}
            }
        };

        // Act
        var result = await fileService.CreateFile(request, default);

        // Assert
        var fileOfDb = italiaTreniDbContext.Files.First(x => x.Name.Equals(request.Name));
        Assert.Equal(request.Name, fileOfDb.Name);
        Assert.Equal(fileOfDb.Id, result.Id);
        Assert.Equal(5, fileOfDb.FileDatas.Count);
    }

    /// <summary>
    /// The file is present in the database, but with records. New records are inserted for that file, but only one 
    /// is not already present and therefore must be inserted.
    /// </summary>
    /// <returns>Return id of file. </returns>
    [Fact]
    public async Task CreateFile_ShouldReturnSuccess_WhenThereAreDataFilesButRequestContainOnlyOneNewDataFile()
    {
        // Arrange
        CreateStandarFile("inserimento solo dati");
        var request = new Api.Model.Request.CreateFileRequest
        {
            Name = "inserimento solo dati",
            Datas = new List<Model.Request.DataFile>()
            {
                new Model.Request.DataFile(){MM = 1, P1=-5.053467, P2=0, P3=0, P4=0},
                new Model.Request.DataFile(){MM = 2, P1=-4.5561523, P2=0, P3=0, P4=0},
                new Model.Request.DataFile(){MM = 4, P1=-4.4924316, P2=0, P3=0, P4=0}
            }
        };
        int totalNumberOfFiles = file.FileDatas.Count + 1;

        // Act
        var result = await fileService.CreateFile(request, default);

        // Assert
        var fileOfDb = italiaTreniDbContext.Files.First(x => x.Name.Equals(request.Name));
        Assert.Equal(request.Name, fileOfDb.Name);
        Assert.Equal(fileOfDb.Id, result.Id);
        Assert.Equal(totalNumberOfFiles, fileOfDb.FileDatas.Count);
    }

    #endregion
}