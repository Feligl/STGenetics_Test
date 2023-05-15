using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STGeneticsTest.Contracts;
using STGeneticsTest.Models;

namespace STGeneticsTest.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class PurchaseController : ControllerBase
    {
        #region Constructor and Properties
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IPurchaseDetailRepository _purchaseDetailRepository;
        private readonly IAnimalRepository _animalRepository;
        private readonly ILogger<PurchaseController> _logger;

        private readonly int AnimalDiscountThreshold = 50;
        private readonly double AnimalDiscountRate = 0.95;
        private readonly int PurchaseDiscountThreshold = 200;
        private readonly double PurchaseDiscountRate = 0.97;
        private readonly int FreeFreightThreshold = 300;
        private readonly int BaseFreightValue = 1000;

        public PurchaseController(
            ILogger<PurchaseController> logger, 
            IAnimalRepository animalRepository, 
            IPurchaseRepository purchaseRepository,
            IPurchaseDetailRepository purchaseDetailRepository)
        {
            _logger = logger;
            _purchaseRepository = purchaseRepository;
            _animalRepository = animalRepository;
            _purchaseDetailRepository = purchaseDetailRepository;
        }
        #endregion

        #region Public Methods & Endpoints
        [HttpPost]
        public async Task<IActionResult> PurchaseAnimals(PurchaseDto arg)
        {
            var (checkPurchase, uniqueAnimalIdList) = this.checkPurchase(arg);
            if (checkPurchase != null) return checkPurchase;

            var uniquePurchaseDetailList = CreateListWithQuantities(arg, uniqueAnimalIdList);

            try
            {
                var listWithDetails = await _animalRepository.GetAnimalsByIdList(uniquePurchaseDetailList);

                var purchaseDetailListWithPrices = new List<PurchaseDetailWithPriceDto>();
                foreach (var item in listWithDetails)
                {
                    purchaseDetailListWithPrices.Add(new PurchaseDetailWithPriceDto
                    {
                        AnimalId = item.Id,
                        QuantitySold = item.Quantity,
                        UnitPrice = item.Quantity > AnimalDiscountThreshold ? item.Price * AnimalDiscountRate : item.Price
                    });
                }

                int totalQuantity = 0;
                double totalOrderPrice = 0;
                int freightValue = BaseFreightValue;

                foreach (var item in purchaseDetailListWithPrices)
                {
                    totalOrderPrice += item.QuantitySold * item.UnitPrice;
                    totalQuantity += item.QuantitySold;
                }

                if (totalQuantity > PurchaseDiscountThreshold)
                    totalOrderPrice *= PurchaseDiscountRate;

                if (totalQuantity > FreeFreightThreshold)
                    freightValue = 0;

                var purchase = new PurchaseInfoDto
                {
                    Freight = freightValue,
                    TotalOrderPrice = totalOrderPrice
                };

                var purchaseDb = await _purchaseRepository.InsertPurchase(purchase);

                foreach (var item in purchaseDetailListWithPrices)
                {
                    var purchaseDetail = new PurchaseDetail
                    {
                        AnimalId = item.AnimalId,
                        PurchaseId = purchaseDb.PurchaseId,
                        UnitPrice = item.UnitPrice,
                        QuantitySold = item.QuantitySold
                    };
                    await _purchaseDetailRepository.InsertPurchaseDetail(purchaseDetail);
                }                

                return Ok(purchaseDb);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        private List<PurchaseDetailDto> CreateListWithQuantities(PurchaseDto purchase, HashSet<int> idList)
        {
            var result = new List<PurchaseDetailDto>();
            foreach (var id in idList)
            {
                result.Add(new PurchaseDetailDto
                {
                    AnimalId = id,
                    QuantitySold = purchase.AnimalPurchased.First(p => p.AnimalId == id).QuantitySold
                });
            }
            return result;
        }

        #endregion

        #region Private Methods & Helpers
        private (IActionResult, HashSet<int>) checkPurchase(PurchaseDto animalList)
        {
            IActionResult error = null;
            var animalIdList = new HashSet<int>();
            foreach (var animal in animalList.AnimalPurchased)
            {
                if (animal.AnimalId == 0)
                {
                    error = BadRequest($"\"AnimalId\" should not be empty");
                    break;
                }

                if (animal.QuantitySold <= 0)
                {
                    error = BadRequest($"\"Quantity\" can't be zero or negative");
                    break;
                }

                if (!animalIdList.Add(animal.AnimalId))
                {
                    error = BadRequest($"The animalId {animal.AnimalId} is repeated and orders with duplications can't be processed...");
                    break;
                }
            }

            return (error, animalIdList);
        }
        #endregion
    }
}