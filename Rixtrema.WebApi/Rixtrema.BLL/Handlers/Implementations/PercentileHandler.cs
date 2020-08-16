using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Rixtrema.BLL.Handlers.Interfaces;
using Rixtrema.BLL.Helpers;
using Rixtrema.DAL.EF.Services.Implementations;
using Rixtrema.DAL.EF.Services.Interfaces;
using Rixtrema.DAL.Models.Request;
using Rixtrema.DAL.Models.Response;

namespace Rixtrema.BLL.Handlers.Implementations
{
public class PercentileHandler : IPercentileHandler
    {

        private readonly IConfiguration _configuration;
        
        public PercentileHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> CompletePercentile(int type = 0)
        {
            try
            {
                var percentileForInsert = new List<PercentileCreateRequest>();
                
                List<GetPlansResponse> plansRecords;
                using (IDbService dbService = new DbService(_configuration).DbServiceInstance)
                {
                    var getPlans = await dbService.Plans.GetRange();
                    if (getPlans.Count == 0)
                        return "Not Found Records int tPlans";

                    plansRecords = getPlans;
                }

                switch (type)
                {
                    default:
                    {
                        var getPercentileForBucket = await PercentileForBucket(plansRecords);
                        percentileForInsert.AddRange(getPercentileForBucket);
                        
                        List<GetStateResponse> states;
                        using (IDbService dbService = new DbService(_configuration).DbServiceInstance)
                        {
                            var getStates = await dbService.State.GetRange();
                            if (getStates.Count == 0)
                                return "Not Found Records int tStates";

                            states = getStates;
                        }

                        var getPercentileForState = await PercentileForState(plansRecords, states);
                        percentileForInsert.AddRange(getPercentileForState);
                        
                        var getPercentileForBusinessCode = await PercentileForBusinessCode(plansRecords);
                        percentileForInsert.AddRange(getPercentileForBusinessCode);
                        
                        break;
                    }
                    case 1:
                    {
                        var getPercentileForBucket = await PercentileForBucket(plansRecords);
                        percentileForInsert.AddRange(getPercentileForBucket);
                        
                        break;
                    }
                    case 2:
                    {
                        List<GetStateResponse> states;
                        using (IDbService dbService = new DbService(_configuration).DbServiceInstance)
                        {
                            var getStates = await dbService.State.GetRange();
                            if (getStates.Count == 0)
                                return "Not Found Records int tStates";

                            states = getStates;
                        }

                        var getPercentileForState = await PercentileForState(plansRecords, states);
                        percentileForInsert.AddRange(getPercentileForState);
                        
                        break;
                    }
                    case 3:
                    {
                        var getPercentileForBusinessCode = await PercentileForBusinessCode(plansRecords);
                        percentileForInsert.AddRange(getPercentileForBusinessCode);
                        
                        break;
                    }
                }

                if (percentileForInsert.Count == 0) 
                    return "Not percentile for inset";

                using (IDbService dbService = new DbService(_configuration).DbServiceInstance)
                {
                    var createPercentile = await dbService.Percentile.CreateRange(percentileForInsert);
                    return createPercentile;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<List<PercentileCreateRequest>> PercentileForBucket(List<GetPlansResponse> plansRecords,
            int nTileGroupCount = 100)
        {
            var result = new List<PercentileCreateRequest>();
            
            const BindingFlags bindingFlags = BindingFlags.Instance |
                                              BindingFlags.NonPublic |
                                              BindingFlags.Public;
            try
            {
                for (var i = 0; i < 29; i++)
                {
                    var listPropertiesNames = plansRecords.GetType().GetProperties(bindingFlags).Select(field => field.Name).ToList();
                    
                    foreach (var propertyName in listPropertiesNames)
                    {
                        var maxValue = (float?)plansRecords.Max(pr => pr.GetType().GetProperties(bindingFlags)
                            .First(property => property.Name.Equals(propertyName)).GetValue(plansRecords));

                        var bucketId = i;
                        var nTileGroupingForPercentile = plansRecords.AsQueryable().OrderByDynamic(propertyName).ToList()
                            .Select((value,index) => new {Value = value, Index = index})
                            .GroupBy(c => Math.Floor(c.Index / (plansRecords.Count / (double)nTileGroupCount)), c => 
                            c.Value)
                            .Select(pr => new PercentileCreateRequest
                            {
                                Type = propertyName,
                                Val = maxValue,
                                Bucket = bucketId,
                                Perc = (int)pr.Key
                            });
                        
                        result.AddRange(nTileGroupingForPercentile);
                    }
                }
            }
            catch (Exception)
            {
                return new List<PercentileCreateRequest>();
            }
            
            return result;
        }
        
        
        private async Task<List<PercentileCreateRequest>> PercentileForState(List<GetPlansResponse> plansRecords, 
        List<GetStateResponse> states, int nTileGroupCount = 100)
        {
            var result = new List<PercentileCreateRequest>();
            
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            
            try
            {
                foreach (var state in states)
                {
                    {
                        var listPropertiesNames = plansRecords.GetType().GetProperties(bindingFlags)
                            .Select(field => field.Name).ToList();

                        foreach (var propertyName in listPropertiesNames)
                        {
                            var maxValue = (float?) plansRecords.Max(pr => pr.GetType().GetProperties(bindingFlags)
                                .First(property => property.Name.Equals(propertyName)).GetValue(plansRecords));
                            
                            var nTileGroupingForPercentile = plansRecords.AsQueryable().OrderByDynamic(propertyName)
                                .ToList()
                                .Select((value, index) => new {Value = value, Index = index})
                                .GroupBy(c => Math.Floor(c.Index / (plansRecords.Count / (double) nTileGroupCount)),
                                    c =>
                                        c.Value)
                                .Select(pr => new PercentileCreateRequest
                                {
                                    Type = propertyName,
                                    Val = maxValue,
                                    State = state.Code,
                                    Perc = (int) pr.Key
                                });

                            result.AddRange(nTileGroupingForPercentile);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return new List<PercentileCreateRequest>();
            }
            
            return result;
        }
        
        private async Task<List<PercentileCreateRequest>> PercentileForBusinessCode(List<GetPlansResponse> plansRecords,
            int nTileGroupCount = 100)
        {
            var result = new List<PercentileCreateRequest>();
            
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            try
            {
                var businessCodes = plansRecords.GroupBy(pr => pr.BusinessCode).Where(pr => pr.ToList().Count > 50)
                .Select(bc => bc.Key).ToList();
                
                foreach (var businessCode in businessCodes)
                {
                    {
                        var listPropertiesNames = plansRecords.GetType().GetProperties(bindingFlags)
                            .Select(field => field.Name).ToList();

                        foreach (var propertyName in listPropertiesNames)
                        {
                            var maxValue = (float?) plansRecords.Max(pr => pr.GetType().GetProperties(bindingFlags)
                                .First(property => property.Name.Equals(propertyName)).GetValue(plansRecords));
                            
                            var nTileGroupingForPercentile = plansRecords.AsQueryable().OrderByDynamic(propertyName)
                                .ToList()
                                .Select((value, index) => new {Value = value, Index = index})
                                .GroupBy(c => Math.Floor(c.Index / (plansRecords.Count / (double) nTileGroupCount)),
                                    c =>
                                        c.Value)
                                .Select(pr => new PercentileCreateRequest
                                {
                                    Type = propertyName,
                                    Val = maxValue,
                                    BusinessCode = businessCode,
                                    Perc = (int) pr.Key
                                });

                            result.AddRange(nTileGroupingForPercentile);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return new List<PercentileCreateRequest>();
            }
            
            return result;
        }
        
    }
}