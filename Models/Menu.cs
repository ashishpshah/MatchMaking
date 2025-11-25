using System;
using System.Collections.Generic;

namespace MatchMaking.Models
{
	public partial class Menu : EntitiesBase
	{
		public override long Id { get; set; }
		public long ParentId { get; set; }
		public string? Area { get; set; }
		public string? Controller { get; set; }
		public string? Url { get; set; }
		public string? Name { get; set; }
		public string? Icon { get; set; }
		public int? DisplayOrder { get; set; }
		public bool? IsSuperAdmin { get; set; }
		public bool? IsAdmin { get; set; }
	}
}
