namespace TombProspectors.Database.Models
{
	using LinqToDB.Mapping;

	[Table(Name = "UserHistory")]
	public class UserHistory
	{
		[PrimaryKey, Identity]
		public int Id { get; set; }

		[Column, NotNull]
		public string UserName { get; set; }

		[Column,NotNull]
		public string Target { get; set; }

		[Column, NotNull]
		public string Action { get; set; }

		[Column, NotNull]
		public string Value { get; set; }

		[Column, NotNull]
		public System.DateTime Created { get; set; }
	}
}
