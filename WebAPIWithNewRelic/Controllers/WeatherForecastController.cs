using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using WebAPIWithNewRelic.Models;
using WebAPIWithNewRelic.Services;

namespace WebAPIWithNewRelic.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController(ILogger<WeatherForecastController> logger,
                                           WeatherService weatherService) : ControllerBase
    {
        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get(string location, bool getAQI)
        {
            try
            {
                return Ok(await weatherService.GetWeatherAsync(location, getAQI));
            }
            catch (Exception ex)
            {
                logger.LogError("An exception has occurred in {ServiceName}, {Message}, {StackTrace}",
                             nameof(WeatherService),
                             ex.Message,
                             ex.StackTrace
                             );

                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
