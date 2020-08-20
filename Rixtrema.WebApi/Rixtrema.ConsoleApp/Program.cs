using System;
using System.IO;
using Castle.Core.Internal;
using Microsoft.Extensions.Configuration;
using Rixtrema.BLL.Handlers.Implementations;

namespace Rixtrema.ConsoleApp
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // ReSharper disable once ArrangeTypeModifiers
    class CompletePercentile
    {
        private const string AppSettingsFile = "appsettings.json";
        
        private static IConfiguration _configuration;


        // ReSharper disable once UnusedParameter.Local
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(AppSettingsFile, true, true);
            _configuration = builder.Build();
            
            Console.WriteLine("Please Chose type Complete Percentile 0)by all type 1) byBucket 2) by State 3) by BC");
            if (!int.TryParse(Console.ReadLine(), out var sourceType))
            {
                Console.WriteLine("Input date is wrong please restart and try again");
                return;
            }
            
            var completePercentile = new PercentileHandler(_configuration).CompletePercentile(sourceType).Result;
            Console.WriteLine(completePercentile.IsNullOrEmpty() ? "Percentile complete Failed" : completePercentile);
        }
        
    }
}