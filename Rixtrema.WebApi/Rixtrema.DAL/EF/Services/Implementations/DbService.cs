using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Rixtrema.DAL.EF.Contexts;
using Rixtrema.DAL.EF.Repositories.Implementations;
using Rixtrema.DAL.EF.Repositories.Interfaces;
using Rixtrema.DAL.EF.Services.Interfaces;

namespace Rixtrema.DAL.EF.Services.Implementations
{
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class DbService : IDbService
    {
        private readonly BaseContext _db;

        private readonly Lazy<DbService> _lazyDbService;
        
        private readonly IConfiguration _configuration;

        private const string ConnectionString = "AppSettings:ConnectionString";

        private const string MaxConnectionRetriesNumber = "AppSettings:DatabaseCustomSettings:MaxConnectionRetriesNumber";

        private const string MaxTimeBetweenRetriesInSeconds = "AppSettings:DatabaseCustomSettings:MaxTimeBetweenRetriesInSeconds";
        
        private readonly object _locker = new object();

        private bool _disposed;
        

        private IPercentileRepository _percentileRepository;

        private IStateRepository _stateRepository;

        private IPlansRepository _plansRepository;
        

        public DbService(IConfiguration configuration)
        {
            _configuration = configuration;
            lock (_locker)
            {
                var optionsBuilder = new DbContextOptionsBuilder<BaseContext>();
                try
                {
                    optionsBuilder.UseSqlServer(_configuration[ConnectionString], option =>
                        option.EnableRetryOnFailure(int.Parse(_configuration[MaxConnectionRetriesNumber]),
                            TimeSpan.FromSeconds(int.Parse(_configuration[MaxTimeBetweenRetriesInSeconds])),
                            null));
                    optionsBuilder.UseLazyLoadingProxies(false);
                }
                catch
                {
                    optionsBuilder.UseSqlServer(_configuration[ConnectionString], option =>
                        option.EnableRetryOnFailure(2, TimeSpan.FromSeconds(30), null));
                    optionsBuilder.UseLazyLoadingProxies(false);
                }

                var db = new BaseContext(optionsBuilder.Options);
                _db = db;
                _lazyDbService = new Lazy<DbService>(() => new DbService(_configuration));
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    _db.Dispose();
            }

            _disposed = true;
        }

        public DbService DbServiceInstance => _lazyDbService.Value;
        
        
        public IPercentileRepository Percentile =>
            // ReSharper disable once ConvertToNullCoalescingCompoundAssignment
            _percentileRepository ?? (_percentileRepository = new PercentileRepository(_db));
        
        public IStateRepository State =>
            // ReSharper disable once ConvertToNullCoalescingCompoundAssignment
            _stateRepository ?? (_stateRepository = new StateRepository(_db));
        
        public IPlansRepository Plans =>
            // ReSharper disable once ConvertToNullCoalescingCompoundAssignment
            _plansRepository ?? (_plansRepository = new PlansRepository(_db));


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}