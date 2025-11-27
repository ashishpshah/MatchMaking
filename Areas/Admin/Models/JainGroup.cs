using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatchMaking.Areas.Admin.Models
{
    public class JainGroup : EntitiesBase
	{
		public override long Id { get; set; }
		public string Name { get; set; } = null!;
		[NotMapped] public string User_Id_Str { get; set; }
    }
}
