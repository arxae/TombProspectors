namespace TombProspectors.Database.Models
{
	using LinqToDB.Mapping;

	[Table(Name = "Articles")]
	public class Article
	{
		[PrimaryKey, Identity]
		public int Id { get; set; }

		[Column, NotNull]
		public string Title { get; set; }

		[Column, NotNull]
		public string Content { get; set; }

		[Column, NotNull]
		public string PostedBy { get; set; }

		[Column, NotNull]
		public string Section { get; set; }

		[Column, NotNull]
		public System.DateTime Posted { get; set; }
	}
}
