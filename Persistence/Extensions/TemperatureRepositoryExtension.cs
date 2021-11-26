using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Extensions
{
    public static class TemperatureRepositoryExtension
    {
        public static IQueryable<Temperature> Search(this IQueryable<Temperature> temperatures, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return temperatures;

            var dateTimeSearch = DateTime.Parse(searchTerm);

            return temperatures.Where(t => t.DateTime.Equals(dateTimeSearch));
        }
    }
}
