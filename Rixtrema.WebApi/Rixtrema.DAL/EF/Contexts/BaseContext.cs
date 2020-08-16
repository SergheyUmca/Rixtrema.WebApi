using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Rixtrema.DAL.EF.Entities;

namespace Rixtrema.DAL.EF.Contexts
{
    public sealed class BaseContext : DbContext
    {
        public BaseContext(DbContextOptions options) : base(options)
        {}

        // All keys and constraints are performed here.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PercentileEntity>().HasNoKey();

            modelBuilder.Entity<StateEntity>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Id).ValueGeneratedOnAdd().Metadata
                    .SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
            });
            
            modelBuilder.Entity<PlanEntity>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Id).ValueGeneratedOnAdd().Metadata
                    .SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
            });
        }

       public DbSet<PercentileEntity> PercentileEntities { get; set; }
       
       public DbSet<StateEntity> StateEntities { get; set; }
       
       public DbSet<PlanEntity> PlanEntities { get; set; }
    }
}