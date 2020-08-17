using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using ObjectCloner.Extensions;
using Rixtrema.DAL.EF.Contexts;
using Rixtrema.DAL.EF.Entities;
using Rixtrema.DAL.EF.Repositories.Interfaces;
using Rixtrema.DAL.Models.Request;

namespace Rixtrema.DAL.EF.Repositories.Implementations
{
    public class PercentileRepository : IPercentileRepository
    {
        private readonly BaseContext _db;

        public PercentileRepository(BaseContext db)
        {
            _db = db;
        }
        
        public async Task<string> CreateRange(List<PercentileCreateRequest> entitiesList)
        {
            var result = "Success";
            
            var clonedEntities = entitiesList.ShallowClone();
            
            var strategy = _db.Database.CreateExecutionStrategy();
            await strategy.Execute(async () =>
            {
                await using var transaction = await _db.Database.BeginTransactionAsync();
                // ReSharper disable once CollectionNeverQueried.Local
                var entitiesToBeCreated = new List<PercentileEntity>();
                try
                {
                    var bulkConfig = new BulkConfig
                    {
                        PreserveInsertOrder = true,
                        BulkCopyTimeout = 0
                    };
                        
                        
                    entitiesToBeCreated.AddRange(clonedEntities.Select(entity => new PercentileEntity
                    {
                        Type = entity.Type,
                        Val = entity.Val,
                        Bucket = entity.Bucket,
                        BusinessCode = entity.BusinessCode,
                        Perc = entity.Perc,
                        State = entity.State
                    }));
                        
                    await _db.BulkInsertAsync(entitiesToBeCreated, bulkConfig);
                    await _db.SaveChangesAsync();
                        
                    await transaction.CommitAsync();
                    return result;
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    return $"CreateRange Rollback : {e.Message}";
                }
            });

            return result;
        }
    }
}