
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
    public interface ITemperatureRepository
    {
        Task<IEnumerable<Temperature>> GetAllTempByCityAsync(Guid cityId, TemperatureParameters requestParameters, bool trackChanges, CancellationToken cancellationToken = default);
        Task<Temperature> GetTempByDateAsync(Guid cityId, DateTime temperatureDate, bool trackChanges, CancellationToken cancellationToken = default);
        Task<Temperature> GetTempByIdAsync(Guid cityId, Guid temperatureId, bool trackChanges, CancellationToken cancellationToken = default);
        void Add(Guid cityId, Temperature temperature);
        void Remove(Temperature temperature);
    }
}
