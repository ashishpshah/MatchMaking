using System;
using System.Collections.Generic;

namespace MatchMaking.Models
{
    public partial class RoleMenuAccess
    {
        public long RoleId { get; set; }
        public long MenuId { get; set; }
        public bool IsRead { get; set; }
        public bool IsCreate { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
