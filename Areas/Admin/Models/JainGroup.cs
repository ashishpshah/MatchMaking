using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatchMaking.Areas.Admin.Models
{
    public class JainGroup : EntitiesBase
    {
        [Key]
        public long Id { get; set; }
        public String? Name { get; set; }
        [NotMapped] public string User_Id_Str { get; set; }
    }
}
