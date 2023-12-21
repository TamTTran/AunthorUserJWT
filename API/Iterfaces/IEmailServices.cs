namespace API.Iterfaces
{
	public interface IEmailServices
	{
		public Task SendEmailAsync (string to, string subject, string body);
	}
}
