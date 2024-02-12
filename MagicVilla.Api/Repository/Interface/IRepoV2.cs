using System.Linq.Expressions;

namespace MagicVilla.Api.Repository.Interface
{
    public interface IRepoV2<Class, IdDataType, RetDataType> where Class : class //  jekhane CLASS hocche ekta class
    {
        // Expression < ekhane input and comma diye output ki hobe .. sheta bole dite hoy .. >
        Task<List<Class>> GetAllAsync(Expression<Func<Class, bool>>? filter = null); // output result that can be boolean // this could be null
        Task<Class> GetAsync(Expression<Func<Class, bool>>? filter = null, bool tracked = true); // by defaut true  // filter can be null so.. ? ei ta add korlam
        Task CreateAsync(Class obj);
        Task UpdateAsync(Class obj);
        Task RemoveAsync(Class obj);
        Task SaveAsync();

        
    }
}
