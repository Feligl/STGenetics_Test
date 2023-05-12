using Dapper;
using STGeneticsTest.Contracts;
using STGeneticsTest.Database;
using STGeneticsTest.Models;

namespace STGeneticsTest.Repository
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly DapperContext _context;
        public AnimalRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Animal>> GetAnimals()
        {
            var query = "SELECT * FROM Animals";
            using (var connection = _context.CreateConnection())
            {
                var companies = await connection.QueryAsync<Animal>(query);
                return companies.ToList();
            }
        }
    }
}

