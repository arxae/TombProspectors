namespace TombProspectors
{
	using Microsoft.AspNetCore;
	using Microsoft.AspNetCore.Hosting;

	public class Program
	{
		public static void Main(string[] args) => new Program().Run(args);

		void Run(string[] args)
		{
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.Build()
				.Run();
		}
	}
}
