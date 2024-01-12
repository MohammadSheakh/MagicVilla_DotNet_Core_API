using MagicVilla.Api.Database;
using MagicVilla.Api.Entities;
using MagicVilla.Api.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;

namespace MagicVilla.Api.Repository
{
    public class Repo<Class, IdDataType, RetDataType> : IRepoV2<Class, IdDataType, RetDataType> where Class : class
    {
        // add application db context
        private readonly ApplicationDbContext _db;
        internal DbSet<Class> dbSet;
        public Repo(
            // using  dependency injection
            ApplicationDbContext db
            )
        {
            _db = db;
            this.dbSet = _db.Set<Class>();
        }

        public async Task CreateAsync(Class obj)
        {
            // as db table name can be anything .. 
            // await _db.Villas.AddAsync(obj);
            await dbSet.AddAsync(obj);

            await SaveAsync();
        }

        public async Task<List<Class>> GetAllAsync(Expression<Func<Class, bool>> filter = null)
        {
            // IQueryable<Class> query = _db.Villas;
            IQueryable<Class> query = dbSet;


            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task<Class> GetAsync(Expression<Func<Class, bool>>? filter = null, bool tracked = true)
        {
            // IQueryable<Class> query = _db.Villas;
            IQueryable<Class> query = dbSet;

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

        public async Task RemoveAsync(Class obj)
        {
            //_db.Villas.Remove(obj);

            dbSet.Remove(obj);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Class obj)
        {
            // _db.Villas.Update(obj);
            dbSet.Update(obj);
            await SaveAsync();
        }



        //////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////


        //public async Task<List<Entities.Villa>> GetAllAsync(Expression<Func<Entities.Villa, bool>> filter = null)
        //{
        //    IQueryable<Entities.Villa> query = _db.Villas;

        //    if (filter != null)
        //    {
        //        query = query.Where(filter);
        //    }
        //    return await query.ToListAsync();
        //}

        //public async Task RemoveAsync(Entities.Villa obj)
        //{
        //    _db.Villas.Remove(obj);
        //    await SaveAsync();
        //}

        //public async Task SaveAsync()
        //{
        //    await _db.SaveChangesAsync();
        //    //throw new NotImplementedException();
        //}
        //public async Task<Entities.Villa> GetAsync(Expression<Func<Entities.Villa, bool>> filter = null, bool tracked = true)
        //{
        //    IQueryable<Entities.Villa> query = _db.Villas;

        //    if (!tracked)
        //    {
        //        query = query.AsNoTracking();
        //    }

        //    if (filter != null)
        //    {
        //        query = query.Where(filter);
        //    }
        //    return await query.FirstOrDefaultAsync();
        //}

        //public async Task CreateAsync(Entities.Villa obj)
        //{
        //    await _db.Villas.AddAsync(obj);
        //    await SaveAsync();
        //}

        //public async Task UpdateAsync(Entities.Villa obj)
        //{
        //    _db.Villas.Update(obj);
        //    await SaveAsync();
        //}



    }
}
