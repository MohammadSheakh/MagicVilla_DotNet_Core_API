

using MagicVilla.Api.Entities;
using MagicVilla.Api.Repository.Interface;
using System.Linq.Expressions;

namespace MagicVilla.Api.Modules.Villas.Services.Repository.IRepository
{
    public interface IVillaNumberRepositoryV2 : IRepoV2<VillaNumber, int, VillaNumber>
    {
        // It just contain update method .. 
        // but for now .. we have a common update method .. 
    }
}

