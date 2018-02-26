using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GardenManagement.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Garden> Garden { get; set; }
        public DbSet<Plant> Plant { get; set; }
        public DbSet<Sensor> Sensor { get; set; }
        public DbSet<SensorType> SensorType { get; set; }
        public DbSet<SensorValue> SensorValue { get; set; }
        public DbSet<ReadingParameter> ReadingParameter { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite((ApplicationData.Current.LocalSettings.Values["#ConnectionString"].ToString()));
            optionsBuilder.UseSqlite("Filename=Database.db");
            
        }
        public void RejectChanges()
        {
            //This logic reverts any changes made to entities which are not yet stored to the database itself
            foreach (var entry in ChangeTracker.Entries().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified; //Revert changes made to deleted entity.
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
        }
    }

}