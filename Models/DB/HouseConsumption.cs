using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrickCity.Models.DB
{
    public class HouseConsumption
    {
        public int Id { get; set; }
        public int HouseId { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }
        public double Weather { get; set; }
        public House House { get; set; }
    }
}
