using static WebApplication_API.DapperRepository.BaseEntity;
using System.ComponentModel.DataAnnotations;

namespace WebApplication_API.DapperRepository
{
    public abstract class BaseEntity
    {
        public string GetPrimaryKeyPropertyName()
        {
            var props = this.GetType().GetProperties().Where(x => x.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0).First();
            return props.Name;
        }
    }
}