using Dapper;
using STGeneticsTest.Contracts;
using STGeneticsTest.Database;
using STGeneticsTest.Models;
using System.Data;

namespace STGeneticsTest.Repository
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly DapperContext _context;
        public AnimalRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<Animal> GetAnimalById(int id)
        {
            var query = "SELECT * FROM Animals WHERE AnimalId = @id";
            var queryParams = new DynamicParameters();
            queryParams.Add("id", id, DbType.Int32);

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryFirstAsync<Animal>(query, queryParams);
                return result;
            }
        }

        public async Task<IEnumerable<Animal>> GetAllAnimals()
        {
            var query = "SELECT * FROM Animals";
            using (var connection = _context.CreateConnection())
            {
                var animals = await connection.QueryAsync<Animal>(query);
                return animals.ToList();
            }
        }

        private DynamicParameters addParametersToUsp(AnimalDto animal)
        {
            var storedProcedureArgs = new DynamicParameters();
            storedProcedureArgs.Add("name", animal.Name, DbType.String);
            storedProcedureArgs.Add("breed", animal.Breed, DbType.String);
            storedProcedureArgs.Add("birthdate", animal.BirthDate, DbType.Date);
            storedProcedureArgs.Add("sex", animal.Sex, DbType.String);
            storedProcedureArgs.Add("price", animal.Price, DbType.Int32);
            storedProcedureArgs.Add("status", animal.Status, DbType.Boolean);
            return storedProcedureArgs;
        }

        public async Task<Animal> InsertAnimal(AnimalDto animal)
        {
            var storedProcedureName = "upsInsertAnimal";
            var storedProcedureArgs = addParametersToUsp(animal);

            using (var connection = _context.CreateConnection())
            {
                var request = await connection.QuerySingleOrDefaultAsync<int>(storedProcedureName, storedProcedureArgs, commandType: CommandType.StoredProcedure);
                return animal.ToAnimal(request);
            }
        }

        public async Task<Animal> UpdateAnimal(int id, AnimalDto animal)
        {
            var storedProcedureName = "upsUpdateAnimal";
            var storedProcedureArgs = addParametersToUsp(animal);
            storedProcedureArgs.Add("id", id, DbType.Int32);

            using (var connection = _context.CreateConnection())
            {
                var request = await connection.QuerySingleOrDefaultAsync<Animal>(storedProcedureName, storedProcedureArgs, commandType: CommandType.StoredProcedure);
                return request;
            }
        }

        public async Task<int> DeleteAnimal(int id)
        {
            var storedProcedureName = "upsDeleteAnimal";
            var storedProcedureArgs = new DynamicParameters();
            storedProcedureArgs.Add("id", id, DbType.Int32);

            using (var connection = _context.CreateConnection())
            {
                var request = await connection.QuerySingleOrDefaultAsync<int>(storedProcedureName, storedProcedureArgs, commandType: CommandType.StoredProcedure);
                return request;
            }
        }

    }
}

