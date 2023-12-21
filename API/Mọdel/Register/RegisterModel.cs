using System.ComponentModel.DataAnnotations;

namespace API.Mọdel.Register
{
	public class RegisterModel
	{
		public string UserName { get; set; }
		public string Password { get; set; }
		[Required]
		[EmailAddress(ErrorMessage = "Địa chỉ email không đúng")]
		public string Email {get; set;}
	}
}
