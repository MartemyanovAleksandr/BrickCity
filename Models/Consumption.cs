using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrickCity.Models
{
    public class Consumption
    {
        public int ConsumerId { get; set; } 
        public Consumer Consumer { get; set; }
        
        DateTime Date { get; set; }
        double Value { get; set; }
    }
}
