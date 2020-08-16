
using System.Collections.Generic;
using System.Threading.Tasks;
using Rixtrema.DAL.Models.Request;

namespace Rixtrema.DAL.EF.Repositories.Interfaces
{
    public interface IPercentileRepository
    {
        Task<string> CreateRange(List<PercentileCreateRequest> entitiesList);
    }
}