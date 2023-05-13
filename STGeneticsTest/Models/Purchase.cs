
using Dapper;

namespace STGeneticsTest.Models
{
    public class Purchase
    {
        public int PurchaseId { get; set; }
        public int TotalOrderPrice { get; set; }
        public int Freight { get; set; }
    }

    public class PurchaseDto
    {
        public PurchaseDetailArgDto[] AnimalPurchases { get; set; }
    }

    public class PurchaseAnswerDto
    {
        public int PurchaseId { get; set; }
        public int TotalOrderPrice { get; set; }
    }
}
