using API.Mọdel.User;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.HelpSession
{
	public class GenerateToken
	{
		public static string GenerateJwtToken(AppUser user)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(ClaimTypes.Name, user.Email),
				new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid ().ToString ()),
		
			};

			var keySign = new SymmetricSecurityKey (Encoding.UTF8.GetBytes ("abcuefefejfefefeifeif"));

			var credentials = new SigningCredentials (keySign, SecurityAlgorithms.HmacSha512Signature);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity (claims),
				Expires = DateTime.Now.AddDays (7),
				SigningCredentials = credentials,
				
			};

			var tokenHandler = new JwtSecurityTokenHandler ();

			var token = tokenHandler.CreateToken (tokenDescriptor);

			return tokenHandler.WriteToken (token);
		}	
	}
}
