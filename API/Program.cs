using API.Context;
using API.Iterfaces;
using API.Mọdel.Mail;
using API.Mọdel.User;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration.GetConnectionString("DbUser");
// khai báo thuât toán mã hóa
var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-secret-key"));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDb>(op => op.UseMySql(config, ServerVersion.AutoDetect(config)));
// Đăng ký dịch vụ gửi eamil
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddScoped<IEmailServices, EmailServices>();

// Đăng ký dich vụ opetion
builder.Services.AddOptions();

builder.Services.AddIdentity<AppUser, IdentityRole>(op =>
{
	// Cấu hình password
	op.Password.RequiredUniqueChars = 8;
	op.Password.RequireNonAlphanumeric = false;
	op.Password.RequireUppercase = false;

	// Cấu hình đăng nhập  
	op.User.RequireUniqueEmail = true;
	op.SignIn.RequireConfirmedPhoneNumber = false;

	// Cấu hình lockout
	op.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
	op.Lockout.MaxFailedAccessAttempts = 5;

	// Cấu hình User
	op.User.RequireUniqueEmail = true;
	op.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
})
	.AddEntityFrameworkStores<AppDb>()
	.AddDefaultTokenProviders();

// Đăng ký Authentication 
builder.Services.AddAuthentication(op =>
{
	op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
	.AddJwtBearer(op =>
	{
		op.RequireHttpsMetadata = false;
		op.SaveToken = true;
		op.TokenValidationParameters = new TokenValidationParameters
		{
			// tự cấp token
			ValidateIssuer = false,
			ValidateAudience = false,

			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-secret-key"))
		};
	});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
