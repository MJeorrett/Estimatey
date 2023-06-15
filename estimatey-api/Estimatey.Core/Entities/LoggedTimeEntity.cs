using System.Text.Json.Serialization;

namespace Estimatey.Core.Entities;

public class LoggedTimeEntity
{
    public int Id { get; init; }

    public string FloatId { get; init; } = "";

    public int FloatPersonId { get; init; }
    public FloatPersonEntity FloatPerson { get; init; } = null!;

    public DateOnly Date { get; init; }

    public double Hours { get; init; }

    public bool Locked { get; init; }

    public DateOnly? LockedDate { get; init; }
}
