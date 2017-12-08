﻿namespace TombProspectors
{
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;

	public class Startup
	{
		public IConfiguration Configuration { get; }
		public static string ConnectionString { get; private set; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
			ConnectionString = configuration.GetConnectionString("ChaliceDb");
			LinqToDB.Data.DataConnection.DefaultSettings = new DbContextSettings();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseStaticFiles();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller}/{action}/{glyph?}",
					defaults: new { controller = "Home", action = "Index" });
			});
		}
	}
}