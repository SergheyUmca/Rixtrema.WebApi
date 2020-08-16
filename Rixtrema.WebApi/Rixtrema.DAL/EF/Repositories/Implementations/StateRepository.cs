using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ObjectCloner.Extensions;
using Rixtrema.DAL.EF.Contexts;
using Rixtrema.DAL.EF.Repositories.Interfaces;
using Rixtrema.DAL.Models.Response;

namespace Rixtrema.DAL.EF.Repositories.Implementations
{
    public class StateRepository : IStateRepository
    {
        private readonly BaseContext _db;

        public StateRepository(BaseContext db)
        {
            _db = db;
        }
        
        
        public async Task<List<GetStateResponse>> GetRange()
        {
            try
            {
                var result = await _db.StateEntities
                    .Select(s => new GetStateResponse
                    {
                        Id = s.Id,
                        Code = s.Code,
                        Name = s.Name
                    }).AsNoTracking().ToListAsync();

                return result == null || result.Count == 0
                    ? new List<GetStateResponse>()
                    : result.ShallowClone();
            }
            catch (Exception)
            {
               return new List<GetStateResponse>();
            }
        }
    }
}