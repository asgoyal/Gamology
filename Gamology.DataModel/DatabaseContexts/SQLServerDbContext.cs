using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gamology.DataModel.Models;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gamology.DataModel.DatabaseContexts
{
    public class SQLServerDbContext : DbContext
    {
        public DbSet<Sprite> Sprites { get; set; }

        public SQLServerDbContext()
            : base("SQLServerDatabase")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder
            .Entity<Sprite>()
            .Property(s => s.Name)
            .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Types().Configure(c => c.Ignore("IsDirty"));

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            // update the modified date and if newly created entity then update the creation date
            var newOrModifiedEntityHistory = this.ChangeTracker.Entries()
                .Where(e => e.Entity is IModificationHistory && (e.State == EntityState.Added || e.State == EntityState.Modified))
                .Select(e => e.Entity as IModificationHistory);
            foreach (var history in newOrModifiedEntityHistory)
            {
                history.DateModified = DateTime.Now;
                if (history.DateCreated == DateTime.MinValue)
                {
                    history.DateCreated = DateTime.Now;
                }
            }

            int result = base.SaveChanges();

            // The update to IsDirty is soley for client tracking purposes
            foreach (var history in this.ChangeTracker.Entries()
                .Where(e => e is IModificationHistory)
                .Select(e => e as IModificationHistory))
            {
                history.IsDirty = false;
            }

            return result;
        }
    }
}
