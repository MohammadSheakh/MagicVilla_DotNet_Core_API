using MagicVilla.Api.Services.Villa.Dtos;

namespace MagicVilla.Api.Database
{
    public static class DataStore
    {
        // make this static .. 
        public static List<VillaDTO>  villaList = new List<VillaDTO>
        {
                new VillaDTO {Id = 1, Name = "Pool view"},
                new VillaDTO {Id = 2, Name = "Beach view"},
        };
    }
}
