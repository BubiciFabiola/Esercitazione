using ItaliaTreni.Api.Model.Request;
using ItaliaTreni.Api.Validators;

namespace ItaliaTreni.Api.Tests.Validators;

public class CreateFileRequestValidatorTests
{
    private readonly CreateFileRequestValidator validator;

    public CreateFileRequestValidatorTests()
    {
        validator = new CreateFileRequestValidator();
    }

    [Fact]
    public async Task Validate_ShouldFail_WhenNameIsEmpty()
    {
        // Arrange
        var model = new CreateFileRequest
        {
            Name = string.Empty,
            Datas = new List<DataFile>()
        };

        // Act
        var result = await validator.ValidateAsync(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, err => err.ErrorMessage.Equals("Name cannot be null or empty"));
        Assert.Contains(result.Errors, err => err.ErrorCode.Equals("NotEmptyValidator"));
    }

    [Fact]
    public async Task Validate_ShouldFail_WhenNameIsNull()
    {
        // Arrange
        var model = new CreateFileRequest
        {
            Name = null,
            Datas = new List<DataFile>()
        };

        // Act
        var result = await validator.ValidateAsync(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, err => err.ErrorCode.Equals("NotNullValidator") && err.ErrorMessage.Equals("'Name' must not be empty."));
        Assert.Contains(result.Errors, err => err.ErrorCode.Equals("NotEmptyValidator") && err.ErrorMessage.Equals("Name cannot be null or empty"));
    }

    [Fact]
    public async Task Validate_ShouldSucceed()
    {
        // Arrange
        var model = new CreateFileRequest
        {
            Name = "Import",
            Datas = new List<DataFile>()
        };

        // Act
        var result = await validator.ValidateAsync(model);

        // Assert
        Assert.True(result.IsValid);
    }

}
