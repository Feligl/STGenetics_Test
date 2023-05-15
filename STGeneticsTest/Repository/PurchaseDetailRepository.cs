using Dapper;
using STGeneticsTest.Contracts;
using STGeneticsTest.Utilities;
using STGeneticsTest.Models;
using System.Data;

namespace STGeneticsTest.Repository
{
    public class PurchaseDetailRepository : IPurchaseDetailRepository
    {
        private readonly DbContext _context;
        public PurchaseDetailRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<int> InsertPurchaseDetail(PurchaseDetail purchaseDetail)
        {
            var storedProcedureName = "upsInsertPurchaseDetail";
            var storedProcedureArgs = new DynamicParameters();
            storedProcedureArgs.Add("animalId", purchaseDetail.AnimalId, DbType.Double);
            storedProcedureArgs.Add("purchaseId", purchaseDetail.PurchaseId, DbType.Double);
            storedProcedureArgs.Add("unitPrice", purchaseDetail.UnitPrice, DbType.Double);
            storedProcedureArgs.Add("quantitySold", purchaseDetail.QuantitySold, DbType.Double);

            using (var connection = _context.CreateConnection())
            {
                var request = await connection.QuerySingleOrDefaultAsync<int>(storedProcedureName, storedProcedureArgs, commandType: CommandType.StoredProcedure);
                return request;
            }
        }
    }
}

