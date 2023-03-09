using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ValuedInBE.Models;
using ValuedInBE.Models.Entities;

namespace ValuedInBE.System.Extensions
{
    public static class ChangeTrackerExtensions
    {
        public static void CheckAuditing(this ChangeTracker changeTracker, UserContext userContext)
        {
            changeTracker.DetectChanges();
            IEnumerable<EntityEntry> entities =
                changeTracker
                    .Entries()
                    .Where(t =>
                        t.State == EntityState.Modified
                        || t.State == EntityState.Added
                    );
            if(!entities.Any() ) { return; }

            foreach ( EntityEntry entity in entities )
            {

                if(entity is IAuditCreatedBase entityCreateBase && entity.State == EntityState.Added)
                {
                    entityCreateBase.CreatedOn = DateTimeOffset.UtcNow;
                    entityCreateBase.CreatedBy = userContext.UserID;
                }
                if(entity is IAuditCreateUpdateBase entityUpdateBase && entity.State == EntityState.Modified)
                {
                    entityUpdateBase.UpdatedOn = DateTimeOffset.UtcNow;
                    entityUpdateBase.UpdatedBy = userContext.UserID;
                }
            }

        }
    }
}
