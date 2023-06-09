using Estimatey.Application.Projects.Commands.Create;
using FluentValidation;

namespace Estimatey.Application.Projects.Commands.Update;

public class UpdateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(_ => _.DevOpsName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(256);
    }
}
