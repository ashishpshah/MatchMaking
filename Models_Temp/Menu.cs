using System;
using System.Collections.Generic;

namespace MatchMaking.Models_Temp
{
    public partial class Menu
    {
        public long Id { get; set; }
        public long ParentId { get; set; }
        public string? Area { get; set; }
        public string? Controller { get; set; }
        public string? Url { get; set; }
        public string? Name { get; set; }
        public string? Icon { get; set; }
        public int? DisplayOrder { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool? IsSuperAdmin { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
