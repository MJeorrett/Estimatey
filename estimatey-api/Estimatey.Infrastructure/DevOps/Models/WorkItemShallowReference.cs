namespace Estimatey.Infrastructure.DevOps.Models;

internal record WorkItemShallowReference
{
    public int Id { get; set; }

    public string Url { get; set; } = "";
}
