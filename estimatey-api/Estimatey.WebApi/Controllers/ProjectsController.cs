using Estimatey.Application.Project.Commands.Delete;
using Estimatey.Application.Projects.Commands.Create;
using Estimatey.Application.Projects.Commands.Update;
using Estimatey.Application.Projects.Dtos;
using Estimatey.Application.Projects.Queries.GetById;
using Estimatey.Application.Projects.Queries.List;
using Estimatey.Application.Common.AppRequests;
using Estimatey.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Estimatey.WebApi.Controllers;

[ApiController]
public class ProjectsController : ControllerBase
{
    [HttpPost("api/projects")]
    public async Task<ActionResult<AppResponse<int>>> CreateProject(
        [FromBody] CreateProjectCommand command,
        [FromServices] CreateProjectCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var appResponse = await handler.Handle(command, cancellationToken);

        return appResponse.ToActionResult();
    }

    [HttpGet("api/projects")]
    public async Task<ActionResult<AppResponse<List<ProjectDto>>>> ListProjects(
        [FromServices] ListProjectsQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var appResponse = await handler.Handle(new(), cancellationToken);

        return appResponse.ToActionResult();
    }

    [HttpGet("api/projects/{projectId}")]
    public async Task<ActionResult<AppResponse<ProjectDto>>> GetProjectById(
        [FromRoute] int projectId,
        [FromServices] GetProjectByIdQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var appResponse = await handler.Handle(new() { ProjectId = projectId }, cancellationToken);

        return appResponse.ToActionResult();
    }

    [HttpPut("api/projects/{projectId}")]
    public async Task<ActionResult<AppResponse>> UpdateProject(
        [FromRoute] int projectId,
        [FromBody] UpdateProjectCommand command,
        [FromServices] UpdateProjectCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var appResponse = await handler.Handle(command with { ProjectId = projectId }, cancellationToken);

        return appResponse.ToActionResult();
    }

    [HttpDelete("api/projects/{projectId}")]
    public async Task<ActionResult<AppResponse>> DeleteProject(
        [FromRoute] int projectId,
        [FromServices] DeleteProjectCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var appResponse = await handler.Handle(new() { ProjectId = projectId }, cancellationToken);

        return appResponse.ToActionResult();
    }
}