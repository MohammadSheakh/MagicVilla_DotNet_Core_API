using MagicVilla.Api.Database;
using MagicVilla.Api.Entities;
using MagicVilla.Api.Modules.Villas.Services.Repository.IRepository;
using MagicVilla.Api.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace MagicVilla.Api.Modules.Villas.Services.Repository
{
    public class VillaNumberRepositoryV2 : Repo<VillaNumber, int , VillaNumber> ,IVillaNumberRepositoryV2
    {
        // add application db context
        private readonly ApplicationDbContext _db;
        public VillaNumberRepositoryV2
            (
            // using  dependency injection
            ApplicationDbContext db
            ) : base( db ) 
        //⚫ base class er o jehetu dbcontext er instace lagbe .. tai pass kore dilam 
        {
            _db = db;
        }

        // as we do not have any specific method for villa number 
        // if we have any method in IVillaNumberRepository .. then we can 
        // implement here ! .. 
        
    }
}
