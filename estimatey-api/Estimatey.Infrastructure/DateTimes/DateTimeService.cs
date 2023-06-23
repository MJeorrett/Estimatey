using Estimatey.Application.Common.Interfaces;

namespace Estimatey.Infrastructure.DateTimes;

public class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.Now;

    public DateOnly NowDateOnly => DateOnly.FromDateTime(DateTime.Now);
}