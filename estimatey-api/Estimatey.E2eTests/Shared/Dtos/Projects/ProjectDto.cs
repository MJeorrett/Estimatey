namespace Estimatey.E2eTests.Shared.Dtos.Projects;

public record ProjectDto
{
    public int Id { get; init; }

    public string Title { get; init; } = "";
}
