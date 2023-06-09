using System.ComponentModel.DataAnnotations;

namespace Estimatey.Infrastructure.DevOps;

public class DevOpsOptions
{
    [Required]
    public required string OrganizationName { get; init; }

    [Required]
    public required string ProjectName { get; init; }

    [Required]
    public required string AzureAadTenantId { get; init; }

    [Required]
    public required string ClientId { get; init; }

    [Required]
    public required string ClientSecret { get; init; }
}
