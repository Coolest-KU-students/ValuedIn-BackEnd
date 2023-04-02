using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ValuedInBE.System.PersistenceLayer.Entities;
using ValuedInBE.Users.Models;

namespace ValuedInBE.System.PersistenceLayer.Extensions
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
            if (!entities.Any()) { return; }

            foreach (EntityEntry entity in entities)
            {
                if (entity.Entity is IAuditCreatedBase entityCreateBase && entity.State == EntityState.Added)
                {
                    entityCreateBase.CreatedOn = DateTime.Now;
                    entityCreateBase.CreatedBy = userContext.UserID;
                }
                if (entity.Entity is IAuditCreateUpdateBase entityUpdateBase && entity.State == EntityState.Modified )
                {
                    entityUpdateBase.UpdatedOn = DateTime.Now;
                    entityUpdateBase.UpdatedBy = userContext.UserID;
                }
            }
        }
    }
}
