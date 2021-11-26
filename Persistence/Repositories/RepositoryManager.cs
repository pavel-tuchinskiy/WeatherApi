using Domain.Repositories;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly Lazy<ICityRepository> cityRepository;
        private readonly Lazy<ITemperatureRepository> temperatureRepository;
        private readonly Lazy<IUnitOfWork> unitOfWork;
        private readonly IMemoryCache cache;

        public RepositoryManager(WeatherDbContext dbContext, IMemoryCache cache)
        {
            cityRepository = new Lazy<ICityRepository>(() => new CityRepository(dbContext, cache));
            temperatureRepository = new Lazy<ITemperatureRepository>(() => new TemperatureRepository(dbContext));
            unitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(dbContext));
            this.cache = cache;
        }
        public ICityRepository CityRepository => cityRepository.Value;

        public ITemperatureRepository TemperatureRepository => temperatureRepository.Value;

        public IUnitOfWork UnitOfWork => unitOfWork.Value;
    }
}
