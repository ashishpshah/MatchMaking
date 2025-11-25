using System;
using System.Collections.Generic;

namespace MatchMaking.Models
{
    public partial class Role : EntitiesBase
	{
		public override long Id { get; set; }
		public string Name { get; set; } = null!;
        public int? DisplayOrder { get; set; }
        public bool? IsAdmin { get; set; }
    }
}
