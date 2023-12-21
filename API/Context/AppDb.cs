using API.Mọdel.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Context
{
	// Kế thừa từ IdentityDbContext nên có sẵn các DbSet
	// UserRoles Roles RoleClaimsUsers UserClaims UserLogins UserTokens
	public class AppDb : IdentityDbContext<AppUser>
	{
		public AppDb(DbContextOptions<AppDb> options) : base(options)
		{
		}

		public DbSet<AppUser> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			// Bỏ tiền tố AspNet của các bảng: mặc định các bảng trong IdentityDbContext có
			// tên với tiền tố AspNet như: AspNetUserRoles, AspNetUser ...
			// Đoạn mã sau chạy khi khởi tạo DbContext, tạo database sẽ loại bỏ tiền tố đó
			foreach (var entityType in builder.Model.GetEntityTypes())
			{
				var tableName = entityType.GetTableName();


				if (tableName.StartsWith("AspNet"))
				{
					entityType.SetTableName(tableName.Substring(6));
				}
			}
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);

			
		}


	}
}
