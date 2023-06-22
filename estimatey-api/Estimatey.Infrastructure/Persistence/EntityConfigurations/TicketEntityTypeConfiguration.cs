using Estimatey.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estimatey.Infrastructure.Persistence.EntityConfigurations;

internal class TicketEntityTypeConfiguration : WorkItemEntityTypeConfiguration<TicketEntity>
{
    public override void Configure(EntityTypeBuilder<TicketEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable("Ticket");

        builder.Property(_ => _.Id)
            .HasColumnName("TicketId");
    }
}
