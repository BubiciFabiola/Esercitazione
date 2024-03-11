using FluentValidation;
using ItaliaTreni.Api.Model.Request;

namespace ItaliaTreni.Api.Validators;

public class CreateFileRequestValidator : AbstractValidator<CreateFileRequest>
{
    public CreateFileRequestValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Name cannot be null or empty");
    }
}
