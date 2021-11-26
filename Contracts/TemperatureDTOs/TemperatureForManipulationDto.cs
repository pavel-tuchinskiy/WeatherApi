using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.TemperatureDTOs
{
    public abstract class TemperatureForManipulationDto
    {
        [Required(ErrorMessage = "DateTime is a required field.")]
        public DateTime DateTime { get; set; }

        [Required(ErrorMessage = "Celsius degrees is a required field.")]
        public int CelsiusDegrees { get; set; }
    }
}
