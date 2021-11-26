using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.TemperatureDTOs
{
    public class TemperatureDto
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public int CelsiusDegrees { get; set; }
        public Guid CityId { get; set; }
    }
}
