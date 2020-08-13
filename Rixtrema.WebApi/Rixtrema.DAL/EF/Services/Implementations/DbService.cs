using System;
using Microsoft.EntityFrameworkCore;
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
        

        private IPercentileRepository _percentileRepository;

        private readonly object _locker = new object();

        private bool _disposed;

        public DbService(string configuration)
        {
            var configuration1 = configuration;

            lock (_locker)
            {
                var optionsBuilder = new DbContextOptionsBuilder<BaseContext>();
             
                optionsBuilder.UseSqlServer(configuration1, option =>
                    option.EnableRetryOnFailure(2, TimeSpan.FromSeconds(30), null));
                optionsBuilder.UseLazyLoadingProxies(false);

                var db = new BaseContext(optionsBuilder.Options);
                _db = db;
                _lazyDbService = new Lazy<DbService>(() => new DbService(configuration1));
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


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}