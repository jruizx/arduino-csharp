using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Arduino.Infrastructure;

public static class EntityEntryExtensions
{
    public static void MarkOwnedPropertiesAsUnchanged<T>(this EntityEntry<T> entityEntry) where T : class
    {
        var ownedEntries = entityEntry.References.Where(x => x.TargetEntry != null && x.TargetEntry.Metadata.IsOwned())
            .Select(x => x.TargetEntry).ToArray();

        foreach (var ownedEntry in ownedEntries)
        {
            ownedEntry.State = EntityState.Unchanged;
        }
    }
}