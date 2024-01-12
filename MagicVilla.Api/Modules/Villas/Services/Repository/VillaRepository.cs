using MagicVilla.Api.Database;
using MagicVilla.Api.Entities;
using MagicVilla.Api.Modules.Villas.Services.Repository.IRepository;
using MagicVilla.Api.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace MagicVilla.Api.Modules.Villas.Services.Repository
{
    public class VillaRepository : IVillaRepository
    {
        // add application db context
        private readonly ApplicationDbContext _db;
        public VillaRepository(
            // using  dependency injection
            ApplicationDbContext db
            )
        {
            _db = db;
        }

        // it just contain Update ///////////////////////////////////

        public async Task<Villa> UpdateVillaAsync(Villa obj)
        {
            obj.UpdatedDate = DateTime.Now;
            _db.Villas.Update(obj);
            await SaveAsync();
            // await _db.SaveChangesAsync();
            return obj;
        }

        ////////////////// End ///////////////////////////////////////

        public Villa Create(Villa obj)
        {
            throw new NotImplementedException();
        }

        public async Task CreateVillaAsync(Villa obj)
        {
            await _db.Villas.AddAsync(obj);
            await SaveAsync();
        }

        

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Villa> GetAsync(Expression<Func<Villa, bool>> filter = null, bool tracked = true)
        {
            IQueryable<Villa> query = _db.Villas;

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public List<Villa> Get()
        {
            throw new NotImplementedException();
        }

        public Villa Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Villa>> GetAllAsync(Expression<Func<Villa, bool>> filter = null)
        {
            IQueryable<Villa> query = _db.Villas;

            if(filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task RemoveAsync(Villa obj)
        {
            _db.Villas.Remove(obj);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
            //throw new NotImplementedException();
        }

        public Villa Update(Villa obj)
        {
            throw new NotImplementedException();
        }
    }
}
