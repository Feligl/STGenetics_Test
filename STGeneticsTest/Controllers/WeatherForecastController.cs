using Microsoft.AspNetCore.Mvc;
using STGeneticsTest.Contracts;
using STGeneticsTest.Models;

namespace STGeneticsTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IAnimalRepository _animalRepo;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IAnimalRepository animalRepository)
        {
            _logger = logger;
            _animalRepo = animalRepository;
        }

        [HttpGet(Name = "Miau")]
        public async Task<IActionResult> Test()
        {
            return Ok(await _animalRepo.GetAnimals());
        }
    }
}