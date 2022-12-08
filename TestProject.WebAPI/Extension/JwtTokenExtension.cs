using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TestProject.WebAPI.Extension
{
	public static class JwtTokenExtension
	{
        private static JwtTokenConfig _jwtConfig;
		public static WebApplicationBuilder AddJwtToken(this WebApplicationBuilder builder)
		{
			var defaultConfig = new JwtTokenConfig();
			var _config = builder.Configuration.GetSection(nameof(JwtTokenConfig)).Get<JwtTokenConfig>() ?? defaultConfig;
			_jwtConfig = _config;
			var serviceProvider = builder.Services.BuildServiceProvider();

			builder.Services.AddMvc(o =>
			{
				var policy = new AuthorizationPolicyBuilder(_config.AuthenticationScheme)
					.RequireAuthenticatedUser()
					.Build();

				o.Filters.Add(new AuthorizeFilter(policy));
			});

			var scheme = string.IsNullOrWhiteSpace(_config.AuthenticationScheme) ? JwtBearerDefaults.AuthenticationScheme : _config.AuthenticationScheme;
			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = scheme;
				options.DefaultScheme = scheme;
				options.DefaultChallengeScheme = scheme;
			})

				.AddJwtBearer(options =>
				{
					options.RequireHttpsMetadata = _config.IsRequireHttpsMetadata;

					options.Audience = _config.Audience;

					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = _config.IsValidateIssuer,
						ValidateAudience = _config.IsValidateAudience,
						ValidateLifetime = _config.IsValidateLifetime,
						ValidateIssuerSigningKey = _config.IsValidateIssuerSigningKey,

						ValidIssuer = _config.Issuer,
						ValidAudience = _config.Audience,
						ValidAudiences = new List<string> { _config.Audience },
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config.SecretKey))
					};

					//If required, We can extend events
					options.Events = new JwtBearerEvents
					{
						OnAuthenticationFailed = context =>
						{
							return Task.CompletedTask;
						},
						OnTokenValidated = context =>
						{
							return Task.CompletedTask;
						},
						OnMessageReceived = context =>
						{
							return Task.CompletedTask;
						},
						OnChallenge = context =>
						{
							return Task.CompletedTask;
						}
					};
				});


			//Enable Authorization
			builder.Services.AddAuthorization();

			return builder;
		}

		public static WebApplication UseJwtToken(this WebApplication app)
		{
			app.UseAuthorization();
			app.UseAuthentication();
			return app;
		}

	}
	public class JwtTokenConfig
	{
		public string AuthenticationScheme { get; set; } = "Bearer";
		public string Audience { get; set; }
		public string Issuer { get; set; }
		public int ExpiryInMinutes { get; set; } = 30;
		public string SecretKey { get; set; }
		public bool IsValidateIssuer { get; set; } = true;
		public bool IsValidateAudience { get; set; } = true;
		public bool IsValidateLifetime { get; set; } = true;
		public bool IsValidateIssuerSigningKey { get; set; } = true;
		public bool IsRequireHttpsMetadata { get; set; } = false;
	}
}
