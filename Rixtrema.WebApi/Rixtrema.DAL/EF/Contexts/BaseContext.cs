using Microsoft.EntityFrameworkCore;
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
        }

       public DbSet<PercentileEntity> PercentileEntities { get; set; }
    }
}