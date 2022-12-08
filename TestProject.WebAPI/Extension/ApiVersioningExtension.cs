namespace TestProject.WebAPI.Extension
{
	public static class ApiVersioningExtension
	{
		public static void AddVersioning(this WebApplicationBuilder builder)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));

			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddApiVersioning(config =>
			{
				// Specify the default API Version as 1.0
				config.DefaultApiVersion = new ApiVersion(1, 0);
				// If the client hasn't specified the API version in the request, use the default API version number 
				config.AssumeDefaultVersionWhenUnspecified = true;
				// Advertise the API versions supported for the particular endpoint
				config.ReportApiVersions = true;
			});

			builder.Services.AddVersionedApiExplorer(
			options =>
			{
				options.GroupNameFormat = "'v'VVV";
				options.SubstituteApiVersionInUrl = true;
			});
		}
	}
}
