using Arduino.Common.DomainModel;
using Arduino.Common.DomainModel.DomainEventAggregate;
using Arduino.DomainModel.ProjectAggregate;
using Arduino.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Arduino.Infrastructure;

public class ArduinoContext : DbContext
{
    public ArduinoContext(DbContextOptions<ArduinoContext> options) : base(options)
    {
    }

    public DbSet<PersistentDomainEvent> PersistentDomainEvents { get; set; }
    public DbSet<Project> Projects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new PersistentDomainEventConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectConfiguration());
    }

    public Task<int> SaveOnlyDomainEventsAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var entries = ChangeTracker.Entries<Entity>().ToArray();

        foreach (var entry in entries)
        {
            if (entry.Entity is not PersistentDomainEvent)
            {
                entry.State = EntityState.Unchanged;
            }
        }

        return base.SaveChangesAsync(true, cancellationToken);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        OnBeforeSaving();

        var numberOfItemsWritten = base.SaveChanges(acceptAllChangesOnSuccess);

        OnAfterSaving();

        return numberOfItemsWritten;
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
    {
        OnBeforeSaving();

        var numberOfItemsWritten = base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

        OnAfterSaving();

        return numberOfItemsWritten;
    }

    private void OnBeforeSaving()
    {
        SaveDomainEvents();

        ApplySoftDelete();
    }

    private void OnAfterSaving()
    {

    }

    private void ApplySoftDelete()
    {
        var entries = ChangeTracker.Entries<Entity>().ToArray();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.CurrentValues["IsDeleted"] = false;
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.CurrentValues["IsDeleted"] = true;
                    entry.MarkOwnedPropertiesAsUnchanged();

                    break;
            }
        }
    }

    private void SaveDomainEvents()
    {
        var domainEventEntities = ChangeTracker.Entries<Entity>()
            .Select(x => x.Entity)
            .Where(x => x.DomainEvents.Any())
            .ToArray();

        foreach (var entity in domainEventEntities)
        {
            var events = entity.DomainEvents.ToArray();
            entity.ClearEvents();

            foreach (var domainEvent in events)
            {
                PersistentDomainEvents.Add(new PersistentDomainEvent(domainEvent));
            }
        }
    }
}