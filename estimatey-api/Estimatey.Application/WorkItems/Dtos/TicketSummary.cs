using Estimatey.Core.Entities;

namespace Estimatey.Application.WorkItems.Dtos;

public record TicketSummary
{
    public int Id { get; init; }

    public string Title { get; init; } = "";

    public string State { get; init; } = "";

    public static TicketSummary MapFromEntity(TicketEntity entity)
    {
        return new()
        {
            Id = entity.Id,
            Title = entity.Title,
            State = entity.State,
        };
    }
}
