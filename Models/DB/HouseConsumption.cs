using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BrickCity.Models.DB
{
    public class HouseConsumption
    {
        public int Id { get; set; }
        public int HouseId { get; set; }
        [Display(Name = "Дата")]
        public DateTime Date { get; set; }
        [Display(Name = "Потребление")]
        public double Value { get; set; }
        [Display(Name = "Температура")]
        public double Weather { get; set; }
        public House House { get; set; }
    }
}
