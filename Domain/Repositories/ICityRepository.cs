using Domain.Entities;
using Domain.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface ICityRepository
    {
        Task<IEnumerable<City>> GetAllCitiesAsync(bool includeTemperature, CityParameters requestParameters, bool trackChanges, CancellationToken cancellationToken = default);
        Task<City> GetCityByIdAsync(Guid cityId, bool includeTemperature, bool trackChanges, CancellationToken cancellationToken = default);
        void Add(City city);
        void Remove(City city);
    }
}
