using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using Castle.Core.Internal;
using Microsoft.Extensions.Configuration;
using Rixtrema.BLL.Handlers.Interfaces;
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
            var result = "";
            try
            {
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
                        List<GetStateResponse> states;
                        using (IDbService dbService = new DbService(_configuration).DbServiceInstance)
                        {
                            var getStates = await dbService.State.GetRange();
                            if (getStates.Count == 0)
                                return "Not Found Records int tStates";

                            states = getStates;
                        }
                        
                        var savePercentileForBusinessCodeTask =  SavePercentileForBusinessCode(plansRecords);
                        
                        var savePercentileForBucketTask =  SavePercentileForBucket(plansRecords);
                        
                        var savePercentileForState =  SavePercentileForState(plansRecords, states);

                        result += $" {savePercentileForState.Result} ";
                        result += $" {savePercentileForBucketTask.Result} ";
                        result += $" {savePercentileForBusinessCodeTask.Result} ";

                        break;
                    }
                    case 1:
                    {
                        var savePercentileForBucket = await SavePercentileForBucket(plansRecords);
                        result += $" {savePercentileForBucket} ";
                        
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

                        var savePercentileForState = await SavePercentileForState(plansRecords, states);
                        result += $" {savePercentileForState} ";

                        break;
                    }
                    case 3:
                    {
                        var savePercentileForBusinessCode = await SavePercentileForBusinessCode(plansRecords);
                        result += $" {savePercentileForBusinessCode} ";
                        break;
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<string> SavePercentileForBucket(List<GetPlansResponse> plansRecords,
            int nTileGroupCount = 100)
        {
            var result = new List<PercentileCreateRequest>();
            
            const BindingFlags bindingFlags = BindingFlags.Instance |
                                              BindingFlags.NonPublic |
                                              BindingFlags.Public;
            
            var excludeFields = new List<string>
            {
                "SponsDfeMailState",
                "Id",
                "BusinessCode",
                "Bucket"
            };
            
            try
            {
                //get List propertiesName
                var listPropertiesNames = plansRecords.First().GetType().GetProperties(bindingFlags)
                    .Where(f => !excludeFields.Contains(f.Name)).Select(field => field.Name).ToList();
                
                var groupPercentileTaskList = new List<Task<List<PercentileCreateRequest>>>();
                
                // Get Ntile Groups by bucketId
                for (var i = 1; i < 29; i++)
                {
                    var bucketId = i;
                    
                    // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
                    foreach (var propertyName in listPropertiesNames)
                    {
                        groupPercentileTaskList.Add(await Task.Factory.StartNew(() =>
                            GetPercentileByFieldName(plansRecords, propertyName, nTileGroupCount, null, bucketId)));
                    }
                }
                
                var resultList = groupPercentileTaskList.Select(task => task.Result).ToList();

                result.AddRange(resultList.Where(task => task?.Count > 0).SelectMany(s => s).ToList());

                using IDbService dbService = new DbService(_configuration).DbServiceInstance;
                var createPercentile = await dbService.Percentile.CreateRange(result);
                return createPercentile;
            }
            catch (Exception e)
            {
                return $"SavePercentileForBucket Filed : {e.Message}";
            }
        }
        
        
        private async Task<string> SavePercentileForState(List<GetPlansResponse> plansRecords,
            List<GetStateResponse> states, int nTileGroupCount = 100)
        {
            var result = new List<PercentileCreateRequest>();
            
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            
            var excludeFields = new List<string>
            {
                "SponsDfeMailState",
                "Id",
                "BusinessCode",
                "Bucket"
            };
            
            try
            {
                //get List propertiesName
                var listPropertiesNames = plansRecords.First().GetType().GetProperties(bindingFlags)
                    .Where(f => !excludeFields.Contains(f.Name)).Select(field => field.Name).ToList();

                var groupPercentileTaskList = new List<Task<List<PercentileCreateRequest>>>();
                
                // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
                foreach (var state in states)
                {
                    // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
                    foreach (var propertyName in listPropertiesNames)
                    {
                        groupPercentileTaskList.Add(await Task.Factory.StartNew(() =>
                            GetPercentileByFieldName(plansRecords, propertyName, nTileGroupCount, state.Code)));
                    }
                }
                
                var resultList = groupPercentileTaskList.Select(task => task.Result).ToList();

                result.AddRange(resultList.Where(task => task?.Count > 0).SelectMany(s => s).ToList());

                using IDbService dbService = new DbService(_configuration).DbServiceInstance;
                var createPercentile = await dbService.Percentile.CreateRange(result);
                return createPercentile;
            }
            catch (Exception e)
            {
                return $"PercentileForState Filed : {e.Message}";
            }
        }
        
        private async Task<string> SavePercentileForBusinessCode(List<GetPlansResponse> plansRecords,
            int nTileGroupCount = 100)
        {
            var result = new List<PercentileCreateRequest>();
            
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            
            var excludeFields = new List<string>
            {
                "SponsDfeMailState",
                "Id",
                "BusinessCode",
                "Bucket"
            };
            
            try
            {
                //get List propertiesName
                var listPropertiesNames = plansRecords.First().GetType().GetProperties(bindingFlags)
                    .Where(f => !excludeFields.Contains(f.Name)).Select(field => field.Name).ToList();
                
                var businessCodes = plansRecords.GroupBy(pr => pr.BusinessCode).Where(pr => pr.ToList().Count > 50)
                .Select(bc => bc.Key).ToList();
                
                var groupPercentileTaskList = new List<Task<List<PercentileCreateRequest>>>();
                
                // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
                foreach (var businessCode in businessCodes)
                {
                    foreach (var propertyName in listPropertiesNames)
                    {
                        groupPercentileTaskList.Add(await Task.Factory.StartNew(() =>
                            GetPercentileByFieldName(plansRecords, propertyName, nTileGroupCount, null, null, businessCode)));
                    }
                }
                
                var resultList = groupPercentileTaskList.Select(task => task.Result).ToList();

                result.AddRange(resultList.Where(task => task?.Count > 0).SelectMany(s => s).ToList());

                using IDbService dbService = new DbService(_configuration).DbServiceInstance;
                var createPercentile = await dbService.Percentile.CreateRange(result);
                return createPercentile;
            }
            catch (Exception e)
            {
                return $"SavePercentileForBucket Filed : {e.Message}";
            }
        }
        
        
        private async Task<List<PercentileCreateRequest>> GetPercentileByFieldName(
            List<GetPlansResponse> plansRecords, string fieldName, int nTileGroupCount, string stateCode = null, int? 
            bucketId = null, string businessCode = null)
        {
            try
            {
                const BindingFlags bindingFlags = BindingFlags.Instance |
                                                  BindingFlags.NonPublic |
                                                  BindingFlags.Public;

                //Get Values by fieldName
                var listValues = plansRecords.Where(pr =>
                        (bucketId != null && pr.Bucket == bucketId) ||
                        (!stateCode.IsNullOrEmpty() && pr.SponsDfeMailState.Equals(stateCode)) ||
                        (!businessCode.IsNullOrEmpty() && pr.BusinessCode.Equals(businessCode)))
                    .Select(pr =>
                        pr.GetType().GetProperties(bindingFlags)
                            .Where(property => property.Name.Equals(fieldName))
                            .Select(property => (double?)property.GetValue(pr)).Where( value => value != null))
                    .SelectMany(sm => sm).ToList();

                if(listValues.Count == 0)
                    return new EditableList<PercentileCreateRequest>();
            
                //Ntile Grouping
                var nTileGroupingForPercentile = listValues.OrderByDescending(value => value)
                    .Select((value,index) => new {Value = value, Index = index})
                    .GroupBy(c => Math.Floor(c.Index / (listValues.Count / (double) nTileGroupCount)),
                        c => c.Value)
                    .Select(pr => new PercentileCreateRequest
                    {
                        Type = fieldName,
                        Val = pr.ToList().Max(v => v),
                        Bucket = bucketId,
                        State = stateCode,
                        BusinessCode = businessCode,
                        Perc = (int)pr.Key + 1
                    }).ToList();

                return nTileGroupingForPercentile;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
    }
}