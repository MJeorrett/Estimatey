using Estimatey.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estimatey.Infrastructure.Persistence.EntityConfigurations;

public class TagEntityTypeConfiguration : IEntityTypeConfiguration<TagEntity>
{
    public void Configure(EntityTypeBuilder<TagEntity> builder)
    {
        builder.ToTable("Tag");

        builder.Property(_ => _.Id)
            .HasColumnName("TagId");

        builder.HasMany(_ => _.Features)
            .WithMany(_ => _.Tags)
            .UsingEntity<FeatureTagEntity>(
                r => r.HasOne<FeatureEntity>().WithMany().HasForeignKey(_ => _.FeatureId),
                l => l.HasOne<TagEntity>().WithMany().HasForeignKey(_ => _.TagId));

        builder.HasMany(_ => _.UserStories)
            .WithMany(_ => _.Tags)
            .UsingEntity<UserStoryTagEntity>(
                r => r.HasOne<UserStoryEntity>().WithMany().HasForeignKey(_ => _.UserStoryId),
                l => l.HasOne<TagEntity>().WithMany().HasForeignKey(_ => _.TagId));

        builder.HasMany(_ => _.Tickets)
            .WithMany(_ => _.Tags)
            .UsingEntity<TicketTagEntity>(
                r => r.HasOne<TicketEntity>().WithMany().HasForeignKey(_ => _.TicketId),
                l => l.HasOne<TagEntity>().WithMany().HasForeignKey(_ => _.TagId));

        builder.HasMany(_ => _.Bugs)
            .WithMany(_ => _.Tags)
            .UsingEntity<BugTagEntity>(
                r => r.HasOne<BugEntity>().WithMany().HasForeignKey(_ => _.BugId),
                l => l.HasOne<TagEntity>().WithMany().HasForeignKey(_ => _.TagId));

        builder.HasIndex(_ => _.Name)
            .IsUnique();
    }
}
