using STGeneticsTest.Models;

namespace STGeneticsTest.Contracts
{
    public interface IPurchaseDetailRepository
    {
        public Task<int> InsertPurchaseDetail(PurchaseDetail purchaseDetail);
    }
}
