using Estimatey.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estimatey.Infrastructure.Persistence.EntityConfigurations;

public class TicketTagEntityTypeConfiguration : IEntityTypeConfiguration<TicketTagEntity>
{
    public void Configure(EntityTypeBuilder<TicketTagEntity> builder)
    {
        builder.ToTable("TicketTag");
    }
}
