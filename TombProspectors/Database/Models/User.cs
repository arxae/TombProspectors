namespace TombProspectors.Database.Models
{
	using LinqToDB.Mapping;

	[Table(Name = "Users")]
	public class User
	{
		[Identity, NotNull]
		public int Id { get; set; }

		[PrimaryKey]
		public string UserName { get; set; }

		[Column, NotNull]
		public string Password { get; set; }

		[Column, NotNull]
		public string Email { get; set; }

		[Column, NotNull]
		public string Role { get; set; }

		[Column, NotNull]
		public System.DateTime Created { get; set; }

		[Column, Nullable]
		public System.DateTime LastLogin { get; set; }
	}
}