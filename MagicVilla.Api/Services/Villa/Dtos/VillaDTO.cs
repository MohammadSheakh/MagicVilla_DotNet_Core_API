using System.ComponentModel.DataAnnotations;

namespace MagicVilla.Api.Services.Villa.Dtos
{
    public class VillaDTO
    {
        public int Id { get; set; }
        [Required]
        //[StringLength(50)]
        [MaxLength(4)]
        public string Name { get; set; }
    }
}
