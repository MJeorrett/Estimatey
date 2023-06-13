using Estimatey.Core.Entities;

namespace Estimatey.Application.WorkItems.Dtos;

public record TicketSummaryDto
{
    public int Id { get; init; }

    public string Title { get; init; } = "";

    public string State { get; init; } = "";

    public static TicketSummaryDto MapFromEntity(TicketEntity entity)
    {
        return new()
        {
            Id = entity.Id,
            Title = entity.Title,
            State = entity.State,
        };
    }
}
