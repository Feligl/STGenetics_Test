using Dapper;
using STGeneticsTest.Contracts;
using STGeneticsTest.Utilities;
using STGeneticsTest.Models;
using System.Data;

namespace STGeneticsTest.Repository
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly DbContext _context;
        public PurchaseRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<Purchase> InsertPurchase(PurchaseInfoDto purchase)
        {
            var storedProcedureName = "upsInsertPurchase";
            var storedProcedureArgs = new DynamicParameters();
            storedProcedureArgs.Add("totalOrderPrice", purchase.TotalOrderPrice, DbType.Double);
            storedProcedureArgs.Add("freight", purchase.Freight, DbType.Int32);

            using (var connection = _context.CreateConnection())
            {
                var request = await connection.QuerySingleOrDefaultAsync<int>(storedProcedureName, storedProcedureArgs, commandType: CommandType.StoredProcedure);
                return purchase.ToPurchase(request);
            }
        }
    }
}

