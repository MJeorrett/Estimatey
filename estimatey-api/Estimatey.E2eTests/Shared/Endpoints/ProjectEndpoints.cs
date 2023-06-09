using Estimatey.E2eTests.Shared.Dtos.Projects;
using Estimatey.E2eTests.Shared.Endpoints.Base;

namespace Estimatey.E2eTests.Shared.Endpoints;

internal static class ProjectHttpClientExtensions
{
    public static CreateProjectEndpoint CreateProject(this HttpClient httpClient) => new(httpClient);
    public static GetProjectByIdEndpoint GetProjectById(this HttpClient httpClient) => new(httpClient);
    public static ListProjectsEndpoint ListProjects(this HttpClient httpClient) => new(httpClient);
    public static UpdateProjectEndpoint UpdateProject(this HttpClient httpClient) => new(httpClient);
    public static DeleteProjectEndpoint DeleteProject(this HttpClient httpClient) => new(httpClient);
}

internal class CreateProjectEndpoint : PostApiEndpointWithDto<CreateProjectDto, int>
{
    internal CreateProjectEndpoint(HttpClient httpClient) :
        base(httpClient)
    {
    }

    internal override string BuildPath(CreateProjectDto dto) => $"api/projects";
}

internal class GetProjectByIdEndpoint : GetApiEndpointWithDto<int, ProjectDto>
{
    internal GetProjectByIdEndpoint(HttpClient httpClient) :
        base(httpClient)
    {
    }

    internal override string BuildPath(int projectId) => $"api/projects/{projectId}";
}

internal class ListProjectsEndpoint : GetApiEndpoint<List<ProjectDto>>
{
    internal ListProjectsEndpoint(HttpClient httpClient) :
        base(httpClient, "api/projects")
    {
    }
}

internal class UpdateProjectEndpoint : PutApiEndpointWithDto<UpdateProjectDto, int>
{
    internal UpdateProjectEndpoint(HttpClient httpClient) :
        base(httpClient)
    {
    }

    internal override string BuildPath(UpdateProjectDto dto) => $"api/projects/{dto.Id}";
}

internal class DeleteProjectEndpoint : DeleteApiEndpoint
{
    public DeleteProjectEndpoint(HttpClient httpClient) : base(httpClient)
    {
    }

    public override async Task<HttpResponseMessage> Call(int projectId)
    {
        return await HttpClient.DeleteAsync($"api/projects/{projectId}");
    }
}
