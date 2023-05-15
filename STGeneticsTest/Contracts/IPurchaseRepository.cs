using STGeneticsTest.Models;

namespace STGeneticsTest.Contracts
{
    public interface IPurchaseRepository
    {
        public Task<Purchase> InsertPurchase(PurchaseInfoDto purchase);
    }
}
