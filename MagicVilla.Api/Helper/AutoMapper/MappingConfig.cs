using AutoMapper;
using MagicVilla.Api.Entities;
using MagicVilla.Api.Modules.Villas.Dtos;


namespace MagicVilla.Api.Helper.AutoMapper
{
    public class MappingConfig : Profile // from auto mapper
    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDTO>();
            CreateMap<VillaDTO, Villa>();

            CreateMap<Villa, VillaCreateDTO>().ReverseMap();
            CreateMap<Villa, VillaUpdateDTO>().ReverseMap();
        }

        // we need to register that mapping in services .. 
    }
}
