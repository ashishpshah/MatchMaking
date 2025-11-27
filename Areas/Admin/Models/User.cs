using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatchMaking
{
    public partial class User : EntitiesBase
    {
        public override long Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public int? NoOfWrongPasswordAttempts { get; set; }
        public DateTime? NextChangePasswordDate { get; set; }
        [NotMapped] public string User_Role { get; set; }
        [NotMapped] public long User_Role_Id { get; set; }
        [NotMapped] public long RoleId { get; set; }
        [NotMapped] public bool IsPassword_Reset { get; set; }
        [NotMapped] public DateTime? Date { get; set; }
        [NotMapped] public string Date_Text { get; set; }
        [NotMapped] public string User_Id_Str { get; set; }
        [NotMapped] public string Role_Id_Str { get; set; }
    }
}
