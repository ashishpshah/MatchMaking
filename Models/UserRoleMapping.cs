using System;
using System.Collections.Generic;

namespace MatchMaking.Models
{
    public partial class UserRoleMapping
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
