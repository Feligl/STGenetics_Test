using STGeneticsTest.Models;

namespace STGeneticsTest.Contracts
{
    public interface IRelationaRepository
    {
        public Task<PurchaseDetail> InsertRelation(AnimalDto animal);
    }
}
