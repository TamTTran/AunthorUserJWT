using API.HelpSession;
using API.Mọdel.Login;
using API.Mọdel.Register;
using API.Mọdel.User;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]

	public class AuthorController : ControllerBase
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly IConfiguration _configuration;
		private readonly EmailServices _serviceEmail;

		public AuthorController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration, EmailServices emailServices)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_configuration = configuration;
			_serviceEmail = emailServices;
		}

		[HttpPost("login")]

		public async Task<IActionResult> Login([FromBody] LoginModel model)
		{
			// kiểm tra user, password và token user có tồn tại không, nếu có trả về
			// kết quả đăng nhập thành công, nếu không token thì thông bạn cần đăng ký tài khoản
			if(ModelState.IsValid)
			{
				var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
				if (result.Succeeded)
				{
					var appUser = _userManager.Users.SingleOrDefault(r => r.UserName == model.UserName);
					var token = GenerateToken.GenerateJwtToken(appUser);
					if (appUser != null)
					{
						// send email
						var subject = "Đăng nhập thành công";
						var body = "Bạn đã đăng nhập thành công vào hệ thống";
						await _serviceEmail.SendEmailAsync(model.Email, subject, body);

					}
					return Ok("Đăng nhập thành công");
				}
				return BadRequest("Tên đăng nhập hoặc mật khẩu không đúng");
			}
			return BadRequest(ModelState);
		}
		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterModel model)
		{
			// viết hàm đăng ký user thông qua appuser và generate token
			if (ModelState.IsValid)
			{
				var user = new AppUser
				{
					UserName = model.UserName,
					Email = model.Email,
					PasswordHash = model.Password
					
					
				};
				var result = await _userManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					var token = GenerateToken.GenerateJwtToken(user);
					return Ok($"Đăng ký thành công" + token);
				}
				return BadRequest(result.Errors);
			}
			return BadRequest(ModelState);
		}
	}
}
