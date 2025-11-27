using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatchMaking
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Please enter user name.")] public string Username { get; set; }
		[Required(ErrorMessage = "Please enter password.")] public string Password { get; set; }
		public string User_Type { get; set; }
		public bool RememberMe { get; set; }
	}
	public class RegisterViewModel
	{
		[Required]
		public string Username { get; set; }
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
		[Required]
		[Compare("Password")]
		public string ConfirmPassword { get; set; }
		public string Role { get; set; }
	}
	//public class ForgotPassword
	//{
	//	public string User_Id { get; set; }
	//	public string User_Name { get; set; }
	//	public string Password { get; set; }
	//	public string User_Type { get; set; }
	//	public string Email { get; set; }
	//}
	public class ForgotPassword 
    {
        public long Id { get; set; }
        public string Email { get; set; }
		public string OTP { get; set; }

        // Add these properties to match your table
        public DateTime CreatedAt { get; set; }
        public bool IsUsed { get; set; }
    }
}
