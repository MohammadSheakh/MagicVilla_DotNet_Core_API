

using MagicVilla.Api.Entities;
using MagicVilla.Api.Repository.Interface;
using System.Linq.Expressions;

namespace MagicVilla.Api.Modules.Villas.Services.Repository.IRepository
{
    public interface IVillaRepository : IRepo<Villa, int, Villa>
    {
        Task<List<Villa>> GetAllAsync(Expression<Func<Villa, bool>> filter = null); // output result that can be boolean
        Task<Villa> GetAsync(Expression<Func<Villa, bool>> filter = null, bool tracked = true); // by defaut true 
        Task CreateVillaAsync(Villa obj);
        // Task UpdateVillaAsync(Villa obj);

        Task<Villa> UpdateVillaAsync(Villa obj); // we want to return the updated villa
        Task RemoveAsync(Villa obj);
        Task SaveAsync();
    }
}

