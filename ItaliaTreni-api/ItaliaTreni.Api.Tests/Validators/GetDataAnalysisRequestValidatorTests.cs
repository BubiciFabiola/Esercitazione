using ItaliaTreni.Api.Model.Request;
using ItaliaTreni.Api.Validators;

namespace ItaliaTreni.Api.Tests.Validators;

public class GetDataAnalysisRequestValidatorTests
{
    private readonly GetDataAnalysisRequestValidator validator;

    public GetDataAnalysisRequestValidatorTests()
    {
        validator = new GetDataAnalysisRequestValidator();
    }

    [Fact]
    public async Task Validate_ShouldFail_WhenStartMMIsZero()
    {
        // Arrange
        var model = new GetDataAnalysisRequest
        {
             StartMM = 0,
             EndMM = 1
        };

        // Act
        var result = await validator.ValidateAsync(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, err => err.ErrorMessage.Equals("StartMM cannot be 0"));
    }

    [Fact]
    public async Task Validate_ShouldFail_WhenStartEndMMIsZero()
    {
        // Arrange
        var model = new GetDataAnalysisRequest
        {
            StartMM = 1,
            EndMM = 0
        };

        // Act
        var result = await validator.ValidateAsync(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.True(result.Errors.Count == 2);
        Assert.Contains(result.Errors, err => err.ErrorMessage.Equals("EndMM cannot be 0"));
        Assert.Contains(result.Errors, err => err.ErrorMessage.Equals("EndMM cannot be less to StartMM"));
    }

    [Fact]
    public async Task Validate_ShouldFail_WhenStartEndMMIsLessStartMM()
    {
        // Arrange
        var model = new GetDataAnalysisRequest
        {
            StartMM = 2,
            EndMM = 1
        };

        // Act
        var result = await validator.ValidateAsync(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, err => err.ErrorMessage.Equals("EndMM cannot be less to StartMM"));
    }

    [Fact]
    public async Task Validate_ShouldSucceed_WhenStartMMAndStartEndMMIsEqual()
    {
        // Arrange
        var model = new GetDataAnalysisRequest
        {
            StartMM = 3,
            EndMM = 3
        };

        // Act
        var result = await validator.ValidateAsync(model);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Validate_ShouldSucceed()
    {
        // Arrange
        var model = new GetDataAnalysisRequest
        {
            StartMM = 3,
            EndMM = 5
        };

        // Act
        var result = await validator.ValidateAsync(model);

        // Assert
        Assert.True(result.IsValid);
    }
}
