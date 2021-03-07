using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrickCity.Controllers
{
    public static class Extensions
    {
        public static IActionResult ToJson<T>(this T obj)
        {
            string objJson = JsonConvert.SerializeObject(obj);
            var res = new ContentResult
            {
                Content = objJson,
                ContentType = "application/json"
            };
            return res;
        }
    }
}
