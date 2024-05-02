using System.Data;
using Dapper;
namespace WebApplication_API.DapperRepository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly IDbConnection _dbConnection;
        private readonly IGetPluralName _getPluralNames;

        public BaseRepository(IDbConnection dbConnection, IGetPluralName getPluralNames)
        {
            _dbConnection = dbConnection;
            _getPluralNames = getPluralNames;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbConnection.QueryAsync<T>($"Select * from {_getPluralNames.GetPluralName(typeof(T).Name)}");
        }

    }
}
