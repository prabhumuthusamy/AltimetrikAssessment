using TestProject.Service.Interface;
using TestProject.Service.Service;

namespace TestProject.WebAPI.Extension
{
	public static class DependencyInjectionExtension
	{
		public static WebApplicationBuilder AddServiceInjection(this WebApplicationBuilder builder)
		{
			builder.Services.AddTransient<IUserService, UserService>();
			builder.Services.AddTransient<IAccountService, AccountService>();
			return builder;
		}
	}
}
