using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrickCity.Models
{
    public class Price
    {
        public int Id { get; set; }
        public int PlantId { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }
    }
}
