using Arduino.DomainModel.ProjectAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arduino.Infrastructure.Configurations;

public class ProjectConfiguration : EntityConfiguration<Project>
{
    public override void Configure(EntityTypeBuilder<Project> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name);
    }
}