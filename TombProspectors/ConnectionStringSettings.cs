﻿
namespace TombProspectors
{
	using System.Collections.Generic;
	using LinqToDB.Configuration;

	public class ConnectionStringSettings : IConnectionStringSettings
	{
		public string ConnectionString { get; set; }
		public string Name { get; set; }
		public string ProviderName { get; set; }
		public bool IsGlobal => false;
	}

	public class DbContextSettings : ILinqToDBSettings
	{
		public IEnumerable<IDataProviderSettings> DataProviders
		{
			get { yield break; }
		}

		public string DefaultConfiguration => "SqlServer";
		public string DefaultDataProvider => "SqlServer";

		public IEnumerable<IConnectionStringSettings> ConnectionStrings
		{
			get
			{
				yield return
					new ConnectionStringSettings
					{
						Name = "SqlServer",
						ProviderName = "SqlServer",
						ConnectionString = Startup.ConnectionString
					};
			}
		}
	}
}
