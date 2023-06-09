using FluentValidation;

namespace Estimatey.Application.Projects.Commands.Create;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(_ => _.DevOpsName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(256);
    }
}
