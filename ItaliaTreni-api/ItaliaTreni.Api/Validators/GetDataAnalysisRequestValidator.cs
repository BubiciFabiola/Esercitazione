using FluentValidation;
using ItaliaTreni.Api.Model.Request;

namespace ItaliaTreni.Api.Validators;

public class GetDataAnalysisRequestValidator : AbstractValidator<GetDataAnalysisRequest>
{
    public GetDataAnalysisRequestValidator()
    {
        RuleFor(x => x.StartMM).NotEqual(0).WithMessage("StartMM cannot be 0");
        RuleFor(x => x.EndMM).NotEqual(0).WithMessage("EndMM cannot be 0");
        RuleFor(x => x.StartMM).LessThanOrEqualTo(x => x.EndMM).WithMessage("EndMM cannot be less to StartMM");
    }
}
