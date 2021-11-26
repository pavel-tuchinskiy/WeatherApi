using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.CityDTOs
{
    public abstract class CityForManipulationDto
    {
        [Required(ErrorMessage = " City name is arequired field.")]
        [MaxLength(50, ErrorMessage = "Maximum lengh for city name is 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Average temperature is arequired field.")]
        public double? AvgTemperature { get; set; }

        [Required(ErrorMessage = "Maximal temperature is arequired field.")]
        public int? MaxTemperature { get; set; }

        [Required(ErrorMessage = "Minimal temperature is arequired field.")]
        public int? MinTemperature { get; set; }
    }
}
