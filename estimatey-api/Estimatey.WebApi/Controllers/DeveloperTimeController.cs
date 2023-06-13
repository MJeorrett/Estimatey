using Estimatey.Application.Common.AppRequests;
using Estimatey.Application.DeveloperTime.Dtos;
using Estimatey.Application.DeveloperTime.Queries.ListLoggedTimeByDeveloper;
using Estimatey.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Estimatey.WebApi.Controllers;

[ApiController]
public class DeveloperTimeController : ControllerBase
{
    [HttpGet("api/projects/{projectId}/logged-time")]
    public async Task<ActionResult<AppResponse<List<DeveloperLoggedTime>>>> ListLoggedTimeByDeveloper(
        [FromRoute] int projectId,
        [FromServices] ListLoggedTimeByDeveloperQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(new() { ProjectId = projectId }, cancellationToken);

        return response.ToActionResult();
    }
}
