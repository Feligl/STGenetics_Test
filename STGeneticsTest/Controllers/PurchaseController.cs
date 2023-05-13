using Bogus;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using STGeneticsTest.Contracts;
using STGeneticsTest.Models;

namespace STGeneticsTest.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PurchaseController : ControllerBase
    {
        #region Constructor and Properties
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IAnimalRepository _animalRepository;
        private readonly ILogger<PurchaseController> _logger;

        public PurchaseController(ILogger<PurchaseController> logger, IAnimalRepository animalRepository, IPurchaseRepository purchaseRepository)
        {
            _logger = logger;
            _purchaseRepository = purchaseRepository;
            _animalRepository = animalRepository;
        }
        #endregion

        #region Public Methods & Endpoints
        [HttpPost]
        public async Task<IActionResult> AnimalPurchase(PurchaseDto purchases)
        {
            try
            {
                foreach (var purchase in purchases.AnimalPurchases)
                {
                    var animal = await _animalRepository.GetAnimalById(purchase.AnimalId);
                    if (animal == null) return NotFound($"Animal #{purchase.AnimalId} does not exist");
                }

                foreach (var purchase in purchases.AnimalPurchases)
                {
                    new PurchaseDetailDto

                    var animal = await _animalRepository.GetAnimalById(purchase.AnimalId);
                    if (animal == null) return NotFound($"Animal #{purchase.AnimalId} does not exist");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }



        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var animal = await _animalRepo.GetAnimalById(id);
                if (animal == null)
                    return NotFound();
                else
                    return Ok(animal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ListAnimalsByFilter(AnimalFilterDto filter)
        {
            try
            {
                return Ok(await _animalRepo.GetFilteredAnimals(filter));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _animalRepo.GetAllAnimals());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Insert(AnimalDto animal)
        {
            var checkAnimal = this.checkAnimal(animal);
            if (checkAnimal != null) return checkAnimal;
            try
            {
                return Ok(await _animalRepo.InsertAnimal(animal));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, AnimalDto animal)
        {
            var checkSex = this.checkSexConstraint(animal);
            if (checkSex != null) return checkSex;
            try
            {
                return Ok(await _animalRepo.UpdateAnimal(id, animal));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                return Ok("Number of Deleted Records: " + await _animalRepo.DeleteAnimal(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("{quantity}")]
        public async Task<IActionResult> GenerateAnimalRecords(int quantity)
        {
            var WasSuccesful = await CreateTestingData(quantity);
            if (WasSuccesful)
                return Ok("Operation was successful");
            else
                return BadRequest("Operation wasn't successful...");
        }
        #endregion

        #region Private Methods & Helpers
        private IActionResult checkAnimal(AnimalDto animal)
        {
            if (animal.Name == null) return BadRequest($"\"Name\" should not be empty");
            if (animal.Breed == null) return BadRequest($"\"Breed\" should not be empty");
            if (animal.BirthDate == default(DateTime)) return BadRequest($"\"BirthDate\" should not be empty");
            if (animal.Sex == null) return BadRequest($"\"Sex\" should not be empty");
            if (animal.Price < 0) return BadRequest($"\"Price\" should not be negative");
            return this.checkSexConstraint(animal);
        }

        private IActionResult checkSexConstraint(AnimalDto animal)
        {
            if (animal.Sex == null) return null;
            animal.Sex = animal.Sex.ToUpper();
            if (animal.Sex != "M" && animal.Sex != "F")
            {
                return BadRequest($"Sex is not \"M\" nor \"F\". The animal cannot be inserted");
            }
            return null;
        }

        private async Task<bool> CreateTestingData(int quantity)
        {
            try
            {
                var generatorObject = new Faker<AnimalDto>()
                .RuleFor(a => a.Name, f => f.Person.Random.Word().Humanize(LetterCasing.Title))
                .RuleFor(a => a.Breed, f => f.PickRandom(AnimalData.AnimalBreed))
                .RuleFor(a => a.BirthDate, f => f.Date.Between(DateTime.Now.AddYears(-5), DateTime.Now))
                .RuleFor(a => a.Sex, f => f.PickRandom(AnimalData.AnimalSex))
                .RuleFor(a => a.Price, f => f.Random.Int(1000, 5000))
                .RuleFor(a => a.Status, f => f.Random.Bool());

                var testAnimals = generatorObject.GenerateBetween(quantity, quantity);

                foreach (var animal in testAnimals)
                    await _animalRepo.InsertAnimal(animal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }

            return true;
        }
        #endregion
    }
}