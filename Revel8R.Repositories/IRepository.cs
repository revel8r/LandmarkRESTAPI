using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revel8R.BusinessEntities;

namespace Revel8R.Repositories
{
    public interface IRepository<T, K> where T : BaseEntity where K : BaseKey
    {
        T Read(K key);

        BaseEntityCollection<T> ReadAll();

        BaseEntityCollection<T> ReadAll(K key);

        Result Update(T entity, K key);

        Result Create(T entity);

        Result Delete(T entity);
    }
}
