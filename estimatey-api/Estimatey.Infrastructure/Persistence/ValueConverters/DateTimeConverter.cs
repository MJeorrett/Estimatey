using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Estimatey.Infrastructure.Persistence.ValueConverters;

internal class DateTimeConverter : ValueConverter<DateTime, DateTime>
{
    public DateTimeConverter() : base(
            dateTime => dateTime.ToUniversalTime(),
            dateTime => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc))
    {
    }
}
