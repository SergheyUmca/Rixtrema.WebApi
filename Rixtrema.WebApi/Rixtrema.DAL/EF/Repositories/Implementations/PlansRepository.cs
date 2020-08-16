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
    public class PlansRepository : IPlansRepository
    {
        private readonly BaseContext _db;

        public PlansRepository(BaseContext db)
        {
            _db = db;
        }
        
        public async Task<List<GetPlansResponse>> GetRange()
        {
            try
            {
                var result = await _db.PlanEntities
                    .Select(p => new GetPlansResponse
                    {
                        Id = p.Id,
                        Earnings = p.Earnings,
                        ActPartics = p.ActPartics,
                        Adminexp = p.Adminexp,
                        AdminExpRate = p.AdminExpRate,
                        Assets = p.Assets,
                        AvgBalance = p.AvgBalance,
                        AvgEmpContrib = p.AvgEmpContrib,
                        AvgPartContrib = p.AvgPartContrib,
                        Bucket = p.Bucket,
                        ParticipantContribAmt = p.ParticipantContribAmt,
                        EmplrContribIncomeAmt = p.EmplrContribIncomeAmt,
                        ParticswithBal = p.ParticswithBal,
                        PartRate = p.PartRate,
                        PartContribRate = p.PartContribRate,
                        EmpContribIncomeRate = p.EmpContribIncomeRate,
                        PercRetirees = p.PercRetirees,
                        SponsDfeMailState = p.SponsDfeMailState,
                        BusinessCode = p.BusinessCode
                    }).AsNoTracking().ToListAsync();

                return result == null || result.Count == 0
                    ? new List<GetPlansResponse>()
                    : result.ShallowClone();
            }
            catch (Exception)
            {
                return new List<GetPlansResponse>();
            }
        }
    }
}