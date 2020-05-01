using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RESchool.Models;

namespace RESchool.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(UserManager<ApplicationUser> userManager, ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            try
            {
                var user = _userManager.GetUserAsync(User);
                if (user == null)
                {
                    _logger.Log(LogLevel.Information, $"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }
                else
                {
                    _logger.LogInformation($"User with ID '{ _userManager.GetUserId(User).ToString()}' asked for their personal data.");
                }
            }
            catch (Exception)
            {

                _logger.Log(LogLevel.Error, $"Unable to load user");
            }
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
