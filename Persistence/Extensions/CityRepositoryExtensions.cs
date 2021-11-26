using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Extensions
{
    public static class CityRepositoryExtensions
    {
        public static IQueryable<City> Search(this IQueryable<City> cities, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return cities;

            var lcSearchTerm = searchTerm.Trim().ToLower();

            return cities.Where(c => c.Name.ToLower().Contains(lcSearchTerm));
        }
    }
}
