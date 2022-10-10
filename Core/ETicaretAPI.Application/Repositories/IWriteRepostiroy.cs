using ETicaretAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Repositories
{
    public interface IWriteRepostiroy<T> : IRepository<T> where T : BaseEntity
    {
        Task<bool> AddAsync(T model);
        Task<bool> AddRangeAsync(List<T> datas);
        bool Delete(T model);
        bool DeleteRange(List<T> datas);
        Task<bool> DeleteAsync(string id);
        bool Update(T model);
        Task<int> SaveAsync();
            
    }
}
