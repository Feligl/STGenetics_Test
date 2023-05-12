using STGeneticsTest.Models;

namespace STGeneticsTest.Contracts
{
    public interface IAnimalRepository
    {
        public Task<IEnumerable<Animal>> GetAnimals();
    }
}
