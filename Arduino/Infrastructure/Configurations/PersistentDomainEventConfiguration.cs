using System.Text.Json;
using Arduino.Common.DomainModel.DomainEventAggregate;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arduino.Infrastructure.Configurations;

public class PersistentDomainEventConfiguration: EntityConfiguration<PersistentDomainEvent>
{
    public override void Configure(EntityTypeBuilder<PersistentDomainEvent> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.TypeName);
        builder.Property(x => x.Status).HasConversion<string>();
        builder.Property(x => x.Data);
        builder.Property(x => x.Attempts);
        builder.Property(x => x.Created);
        builder.Property(x => x.LastExecution);
        builder.Property(x => x.Errors)
            .HasConversion(v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null),
                new ValueComparer<List<string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));
    }
}