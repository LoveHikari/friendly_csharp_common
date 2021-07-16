using Hikari.Common.Web.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        //[HttpGet]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    var rng = new Random();
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = rng.Next(-20, 55),
        //        Summary = Summaries[rng.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}
        [HttpGet]
        public object Get()
        {
            //List<string> l = new List<string>();
            //l.Add("PlatformServices.Default.Application.ApplicationBasePath: " + PlatformServices.Default.Application.ApplicationBasePath);
            //l.Add("System.Environment.CurrentDirectory: " + System.Environment.CurrentDirectory);
            //l.Add("Directory.GetCurrentDirectory(): " + Directory.GetCurrentDirectory());
            //l.Add("GetType().Assembly.Location: " + GetType().Assembly.Location);
            //l.Add("Process.GetCurrentProcess().MainModule.FileName: " + Process.GetCurrentProcess().MainModule.FileName);
            //l.Add("AppDomain.CurrentDomain.BaseDirectory: " + AppDomain.CurrentDomain.BaseDirectory);
            var v = ConfigurationManager.GetAppSettings<List<string>>("Server");
            return v;
        }
    }
}
