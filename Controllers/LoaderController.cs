using BrickCity.Models;
using BrickCity.Models.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace BrickCity.Controllers
{
    public class LoaderController : Controller
    {
        private readonly ILogger<LoaderController> _logger;
        private BrickDB _db;
        public LoaderController(ILogger<LoaderController> logger, BrickDB context)
        {
            _db = context;
            _logger = logger;
        }

        [HttpPost]
        public void Upload(IFormFile upload)
        {
            if (upload == null)
                throw new Exception("There are no any files.");

            DataSet ds = new DataSet();
            Stream stream = upload.OpenReadStream();
          
            ds.ReadXml(stream);

            DataTable houses = ds.Tables[0];
            DataTable house = ds.Tables[1];
            DataTable dates = ds.Tables[2];
            DataTable plants = ds.Tables[3];
            DataTable plant = ds.Tables[4];

            foreach (DataRow row in house.Rows)
            {
                int id = row.Field<int>("House_Id");
                string name = row.Field<string>("Name");

                House h = _db.House.SingleOrDefault(h => h.Id == id);
                if (h == null)
                {
                    _db.House.Add(new House
                    {
                        Id = id,
                        Name = name
                    });
                }
                else h.Name = name;

                _db.SaveChanges();
            };
            foreach (DataRow row in plant.Rows)
            {
                int id = row.Field<int>("Plant_Id");
                string name = row.Field<string>("Name");

                Plant p = _db.Plant.SingleOrDefault(h => h.Id == id);
                if (p == null)
                {
                    _db.Plant.Add(new Plant()
                    {
                        Id = id,
                        Name = name
                    });
                }
                else p.Name = name;

                _db.SaveChanges();
            };
            foreach (DataRow row in dates.Rows)
            {
                int? plantId = row.Field<int?>("Plant_Id");
                int? houseId = row.Field<int?>("House_Id");
                DateTime date = Convert.ToDateTime(row.Field<string>("Date"));
                double? weather = GetValueOrNull(row.Field<string>("Weather"));
                double consumption = (double)GetValueOrNull(row.Field<string>("Consumption"));
                double? price = GetValueOrNull(row.Field<string>("Price"));

                if (plantId != null)
                {
                    PlantsConsumption p = _db.PlantsConsumption.SingleOrDefault(pc => pc.PlantId == plantId && pc.Date == date);
                    if (p == null)
                    {
                        _db.PlantsConsumption.Add(new PlantsConsumption
                        {
                            PlantId = (int)plantId,
                            Date = date,
                            Value = consumption,
                            Price = (double)price
                        });
                    }
                    else
                    {
                        p.PlantId = (int)plantId;
                        p.Date = date;
                        p.Value = consumption;
                        p.Price = (double)price;
                    };
                    _db.SaveChanges();
                };
                if (houseId != null)
                {
                    HouseConsumption h = _db.HouseConsumption.SingleOrDefault(hc => hc.HouseId == houseId && hc.Date == date);
                    if (h == null)
                    {
                        _db.HouseConsumption.Add(new HouseConsumption
                        {
                            HouseId = (int)houseId,
                            Date = date,
                            Value = consumption,
                            Weather = (double)weather
                        });
                    }
                    else
                    {
                        h.HouseId = (int)houseId;
                        h.Date = date;
                        h.Value = consumption;
                        h.Weather = (double)weather;
                    };
                    _db.SaveChanges();
                };
            };                        
        }

        private double? GetValueOrNull (string value) {
            if (string.IsNullOrEmpty(value))
                return null;

            return Double.Parse(value, CultureInfo.InvariantCulture);
        }
    }
}
