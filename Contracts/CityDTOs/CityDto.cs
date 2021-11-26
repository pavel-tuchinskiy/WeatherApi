using Contracts.TemperatureDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.CityDTOs
{
    public class CityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<TemperatureDto> Temperatures { get; set; }
        public int? CurrentTemerature { get; set; }
        public double? AvgTemperature { get; set; }
        public int? MaxTemperature { get; set; }
        public int? MinTemperature { get; set; }
    }
}
