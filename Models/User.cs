using System;
using System.Collections.Generic;

namespace MatchMaking.Models
{
    public partial class User
    {
        public long Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public int? NoOfWrongPasswordAttempts { get; set; }
        public DateTime? NextChangePasswordDate { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
