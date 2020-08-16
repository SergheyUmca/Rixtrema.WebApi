using System;
using Rixtrema.DAL.EF.Repositories.Interfaces;
using Rixtrema.DAL.EF.Services.Implementations;

namespace Rixtrema.DAL.EF.Services.Interfaces
{
    public interface IDbService : IDisposable
    {
        DbService DbServiceInstance { get; }
        
        

        IPercentileRepository Percentile { get; }
        
        IStateRepository State { get; }
        
        IPlansRepository Plans { get; }
    }
}