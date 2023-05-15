using Dapper;
using STGeneticsTest.Contracts;
using STGeneticsTest.Utilities;
using STGeneticsTest.Models;
using System.Data;
using System.Text;

namespace STGeneticsTest.Repository
{
    public class AnimalRepository : IAnimalRepository
    {
        #region Constructor and Properties
        private readonly DbContext _context;
        public AnimalRepository(DbContext context)
        {
            _context = context;
        }
        #endregion

        #region Public Methods
        public async Task<IEnumerable<Animal>> GetAllAnimals()
        {
            var query = "SELECT * FROM Animals";
            using (var connection = _context.CreateConnection())
            {
                var animals = await connection.QueryAsync<Animal>(query);
                return animals.ToList();
            }
        }

        public async Task<Animal> GetAnimalById(int id)
        {
            var storedProcedureName = "upsFindAnimalById";
            var storedProcedureArgs = new DynamicParameters();
            storedProcedureArgs.Add("id", id, DbType.Int32);

            using (var connection = _context.CreateConnection())
            {
                var request = await connection.QuerySingleOrDefaultAsync<Animal>(storedProcedureName, storedProcedureArgs, commandType: CommandType.StoredProcedure);
                return request;
            }
        }

        public async Task<Animal> InsertAnimal(AnimalDto animal)
        {
            var storedProcedureName = "upsInsertAnimal";
            var storedProcedureArgs = AddParametersToUsp(animal);

            using (var connection = _context.CreateConnection())
            {
                var request = await connection.QuerySingleOrDefaultAsync<int>(storedProcedureName, storedProcedureArgs, commandType: CommandType.StoredProcedure);
                return animal.ToAnimal(request);
            }
        }

        public async Task<Animal> UpdateAnimal(int id, AnimalDto animal)
        {
            var storedProcedureName = "upsUpdateAnimal";
            var storedProcedureArgs = AddParametersToUsp(animal);
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

        public async Task<IEnumerable<Animal>> GetAnimalsByFilter(AnimalFilterDto filter)
        {
            var query = "SELECT * FROM Animals WHERE " + GenerateDynamicFilter(filter);
            var storedProcedureArgs = new DynamicParameters();
            storedProcedureArgs.Add("id", filter.AnimalId, DbType.Int32);
            storedProcedureArgs.Add("name", filter.Name, DbType.String);
            storedProcedureArgs.Add("sex", filter.Sex, DbType.String);
            storedProcedureArgs.Add("status", filter.Status, DbType.Boolean);

            using (var connection = _context.CreateConnection())
            {
                var animals = await connection.QueryAsync<Animal>(query, storedProcedureArgs);
                return animals.ToList();
            }
        }

        public async Task<IEnumerable<AnimalPurchaseDto>> GetAnimalsByIdList(List<PurchaseDetailDto> list)
        {
            var query = $"SELECT T1.Id, T2.Price, T1.Quantity, T2.Status " +
                        $"FROM (VALUES {GenerateValues(list)}) AS t1(Id, Quantity) " +
                        $"LEFT JOIN Animals T2 ON t1.Id = t2.AnimalId";

            using (var connection = _context.CreateConnection())
            {
                var animals = await connection.QueryAsync<AnimalPurchaseDto>(query);
                var dbList = animals.ToList();
                var result = dbList.Where(l => l.Price != null).ToList();
                return result;
            }
        }
        #endregion

        #region Private Methods
        private DynamicParameters AddParametersToUsp(AnimalDto animal)
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

        // ↓ The same could have been done using sp_executesql, but Execution Plan is not optimized for that one either...
        private string GenerateDynamicFilter(AnimalFilterDto filter)
        {
            var result = new StringBuilder();
            if (filter.AnimalId != null) result.Append("AnimalId = @id OR ");
            if (filter.Name != null) result.Append("Name LIKE @name OR ");
            if (filter.Sex != null) result.Append("Sex = @sex OR ");
            if (filter.Status != null) result.Append("Status = @status OR ");
            result.Length -= 3;
            return result.ToString();
        }


        private string GenerateValues(List<PurchaseDetailDto> list)
        {
            var result = new StringBuilder();
            foreach (var detail in list) result.Append($"({detail.AnimalId},{detail.QuantitySold}), ");
            result.Length -= 2;
            return result.ToString();
        }
        #endregion
    }
}

