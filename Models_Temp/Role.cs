using System;
using System.Collections.Generic;

namespace MatchMaking.Models_Temp
{
    public partial class Role
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public int? DisplayOrder { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsAdmin { get; set; }
    }
}
