
using Dapper;

namespace STGeneticsTest.Models
{
    public class PurchaseDetail
    {
        public int AnimalId { get; set; }
        public int PurchaseId { get; set; }
        public double UnitPrice { get; set; }
        public int QuantitySold { get; set; }
    }

    public class PurchaseDetailDto
    {
        public int AnimalId { get; set; }
        public int QuantitySold { get; set; }
    }    
    
    public class PurchaseDetailWithPriceDto
    {
        public int AnimalId { get; set; }
        public double UnitPrice { get; set; }
        public int QuantitySold { get; set; }
    }
}
