using System;
using System.Collections.Generic;

namespace MatchMaking.Models
{
    public partial class User : EntitiesBase
    {
        public override long Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public int? NoOfWrongPasswordAttempts { get; set; }
        public DateTime? NextChangePasswordDate { get; set; }
    }
}
