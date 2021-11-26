using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IRepositoryManager
    {
        ICityRepository CityRepository { get; }
        ITemperatureRepository TemperatureRepository { get; }
        IUnitOfWork UnitOfWork { get; }
    }
}
