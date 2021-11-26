using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class City
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int MaxTemperature { get; set; }
        public double AvgTemperature { get; set; }
        public int MinTemperature { get; set; }
        public ICollection<Temperature> Temperatures { get; set; }
    }
}
