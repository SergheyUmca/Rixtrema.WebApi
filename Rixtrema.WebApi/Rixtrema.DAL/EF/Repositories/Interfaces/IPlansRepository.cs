
using System.Collections.Generic;
using System.Threading.Tasks;
using Rixtrema.DAL.Models.Response;

namespace Rixtrema.DAL.EF.Repositories.Interfaces
{
    public interface IPlansRepository
    {
        Task<List<GetPlansResponse>> GetRange();
    }
}