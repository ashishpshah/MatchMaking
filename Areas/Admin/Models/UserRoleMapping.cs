using System;
using System.Collections.Generic;

namespace MatchMaking
{
    public partial class UserRoleMapping : EntitiesBase
	{
		public override long Id { get; set; }
		public long UserId { get; set; }
        public long RoleId { get; set; }
    }
}
