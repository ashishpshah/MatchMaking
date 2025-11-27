namespace MatchMaking.Areas.Admin.Models
{
    public class ChangePasswordViewModel
    {
        public long Id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
