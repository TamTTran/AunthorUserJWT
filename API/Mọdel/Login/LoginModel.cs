using System.ComponentModel.DataAnnotations;

namespace API.Mọdel.Login
{
	public class LoginModel
	{
		[Required(ErrorMessage = "Username is required")]
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Email { get; set;}
	}
}
