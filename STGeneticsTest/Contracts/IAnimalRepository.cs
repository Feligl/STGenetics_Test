using STGeneticsTest.Models;

namespace STGeneticsTest.Contracts
{
    public interface IAnimalRepository
    {
        public Task<Animal> GetAnimalById(int id);
        public Task<IEnumerable<Animal>> GetAllAnimals();
        public Task<Animal> InsertAnimal(AnimalDto animal);
        public Task<Animal> UpdateAnimal(int id, AnimalDto animal);
        public Task<int> DeleteAnimal(int id);
    }
}
