
using Dapper;

namespace STGeneticsTest.Models
{
    public class Purchase
    {
        public int PurchaseId { get; set; }
        public double TotalOrderPrice { get; set; }
        public int Freight { get; set; }
    }

    public class PurchaseDto
    {
        public PurchaseDetailDto[] AnimalPurchased { get; set; }
    }

    public class PurchaseAnswerDto
    {
        public int PurchaseId { get; set; }
        public double TotalOrderPrice { get; set; }
    }

    public class PurchaseInfoDto
    {
        public double TotalOrderPrice { get; set; }
        public int Freight { get; set; }

        public Purchase ToPurchase(int id)
        {
            return new Purchase
            {
                PurchaseId = id,
                TotalOrderPrice = this.TotalOrderPrice,
                Freight = this.Freight,
            };
        }
    }
}
