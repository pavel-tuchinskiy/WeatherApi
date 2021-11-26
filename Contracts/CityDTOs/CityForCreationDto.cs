using Contracts.TemperatureDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.CityDTOs
{
    public class CityForCreationDto : CityForManipulationDto
    {
        public ICollection<TemperatureForCreationDto> Temperatures { get; set; } = new List<TemperatureForCreationDto>();
    }
}
