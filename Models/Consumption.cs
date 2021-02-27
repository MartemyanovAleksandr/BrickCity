using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrickCity.Models
{
    public class Consumption
    {
        public int Id { get; set; } 
        public int ConsumerId { get; set; } 
        public Consumer Consumer { get; set; }

        public DateTime Date { get; set; }
        public double Value { get; set; }
    }
}
