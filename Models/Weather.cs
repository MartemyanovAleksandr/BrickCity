using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrickCity.Models
{
    public class Weather
    {
        int HouseId { get; set; }
        DateTime Date { get; set; }
        double Value { get; set; }
    }
}
