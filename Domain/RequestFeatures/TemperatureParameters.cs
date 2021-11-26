using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RequestFeatures
{
    public sealed class TemperatureParameters : RequestParameters
    {
        public DateTime MinDate { get; set; } = DateTime.MinValue;
        public DateTime MaxDate { get; set; } = DateTime.MaxValue;

        public bool ValidDateRange => MaxDate > MinDate;
    }
}
