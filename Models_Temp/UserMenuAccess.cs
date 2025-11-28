using System;
using System.Collections.Generic;

namespace MatchMaking.Models_Temp
{
    public partial class UserMenuAccess
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public long MenuId { get; set; }
        public bool IsRead { get; set; }
        public bool IsCreate { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }
    }
}
