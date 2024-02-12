using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla.Api.Entities
{
    public class VillaNumber
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]// it's not identity column
        public int VillaNo { get; set; } 
        public string SpecialDetails { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
