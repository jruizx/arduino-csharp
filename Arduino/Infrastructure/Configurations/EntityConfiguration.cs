using Arduino.Common.DomainModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arduino.Infrastructure.Configurations;

public abstract class EntityConfiguration<T> : IEntityTypeConfiguration<T> where T : Entity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.HasKey(c => c.Id);
        builder.Property(x => x.AutoId)
            .ValueGeneratedOnAdd()
            .IsUnicode()
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);


        builder.Property<bool>("IsDeleted");

        builder.Property(x => x.RowVersion).IsRowVersion().IsConcurrencyToken();

        builder.Ignore(c => c.DomainEvents);

        builder.HasQueryFilter(entity =>
            EF.Property<bool>(entity, "IsDeleted") == false);
    }
}