using Estimatey.Application.Common.AppRequests;
using Estimatey.Application.WorkItems.Dtos;
using Estimatey.Application.WorkItems.Queries;
using Estimatey.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Estimatey.WebApi.Controllers;

[ApiController]
public class FeaturesController : ControllerBase
{
    [HttpGet("api/projects/{projectId}/features")]
    public async Task<ActionResult<AppResponse<List<FeatureSummary>>>> ListFeatures(
        [FromRoute] int projectId,
        [FromServices] ListFeaturesQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(new() { ProjectId = projectId }, cancellationToken);

        return response.ToActionResult();
    }
}
