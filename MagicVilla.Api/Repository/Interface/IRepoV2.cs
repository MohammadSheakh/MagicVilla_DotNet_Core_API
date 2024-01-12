using System.Linq.Expressions;

namespace MagicVilla.Api.Repository.Interface
{
    public interface IRepoV2<Class, IdDataType, RetDataType> where Class : class //  jekhane CLASS hocche ekta class
    {
        ////////////////////////////////////////////////////////

        Task<List<Class>> GetAllAsync(Expression<Func<Class, bool>> filter = null); // output result that can be boolean
        Task<Class> GetAsync(Expression<Func<Class, bool>>? filter = null, bool tracked = true); // by defaut true  // filter can be null
        Task CreateAsync(Class obj);
        Task UpdateAsync(Class obj);
        Task RemoveAsync(Class obj);
        Task SaveAsync();

        ////////////////////////////////////////////////////////

        //List<CLASS> Get();
        //CLASS Get(IdDataType id);
        //RetDataType Create(CLASS obj);
        //RetDataType Update(CLASS obj);
        //bool Delete(IdDataType id);

        ////////////////////////////////////////////////////////
    }
}
