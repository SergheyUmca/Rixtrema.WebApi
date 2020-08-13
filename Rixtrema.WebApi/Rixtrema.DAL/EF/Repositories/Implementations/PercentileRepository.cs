using Rixtrema.DAL.EF.Contexts;
using Rixtrema.DAL.EF.Repositories.Interfaces;

namespace Rixtrema.DAL.EF.Repositories.Implementations
{
    public class PercentileRepository : IPercentileRepository
    {
        private readonly BaseContext _db;

        public PercentileRepository(BaseContext db)
        {
            _db = db;
        }
        
    }
}