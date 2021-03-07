using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrickCity.Models.DB
{
    public class PlantsConsumption
    {
        public int Id { get; set; } 
        public int PlantId { get; set; }         
        public DateTime Date { get; set; }
        public double Value { get; set; }
        public double Price { get; set; }
        public Plant Plant { get; set; }
    }
}
