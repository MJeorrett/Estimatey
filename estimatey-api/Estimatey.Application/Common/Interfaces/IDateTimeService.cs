namespace Estimatey.Application.Common.Interfaces;

public interface IDateTimeService
{
    DateTime Now { get; }

    DateOnly NowDateOnly { get; }
}