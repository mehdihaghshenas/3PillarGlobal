using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using Dapper;
using static Azure.Core.HttpHeader;
namespace WebApplication_API.DapperRepository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
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

        public async Task<T?> GetAsyncById(int id)
        {
            var keyName = ((T)Activator.CreateInstance(typeof(T))!).GetPrimaryKeyPropertyName();
            return await _dbConnection.QueryFirstOrDefaultAsync<T>($"Select * from {_getPluralNames.GetPluralName(typeof(T).Name)} where {keyName}=@Id", new { id });
        }

        public async Task<int> Insert(T item)
        {
            List<PropertyInfo> props = new List<PropertyInfo>();
            foreach (var p in item.GetType().GetProperties().Where(x => x.GetCustomAttributes(typeof(NotMappedAttribute), true).Length == 0))
            {
                if (p.Name == item.GetPrimaryKeyPropertyName())
                    continue;
                if (typeof(BaseEntity).IsAssignableFrom(p.PropertyType))
                    continue;
                if (p.PropertyType.IsGenericType && (
                       p.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>) ||
                       p.PropertyType.GetGenericTypeDefinition() == typeof(List<>)))
                {
                    continue;
                }
                else if (p.PropertyType.IsGenericType)
                {
                    continue;
                }
                props.Add(p);
            }
            string insertText = string.Concat(props.Select(x => x.Name+", ").ToArray()).TrimEnd().TrimEnd(',');
            string valueText = string.Concat(props.Select(x => "@" + x.Name+", ").ToArray()).TrimEnd().TrimEnd(',');
            object?[] objects = new object[props.Count];
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i]= props[i].GetValue(item, null);
            }

            return await _dbConnection.ExecuteAsync($"Insert Into ({insertText}) values ({valueText})", objects);
        }
    }
}
