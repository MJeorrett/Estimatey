namespace Estimatey.Application.Common.Configuration;

public record GlobalOptions
{
    public List<int> ExcludedFloatPersonIds { get; set; } = new();
}
