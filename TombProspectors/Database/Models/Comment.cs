namespace TombProspectors.Database.Models
{
	using LinqToDB.Mapping;

	[Table(Name = "Comments")]
	public class Comment
	{
		[PrimaryKey, NotNull, Identity]
		public int Id { get; set; }

		[Column, NotNull]
		public string CommentText { get; set; }

		[Column, NotNull]
		public string Glyph { get; set; }

		[Column, NotNull]
		public string PostedBy { get; set; }

		[Column, NotNull]
		public System.DateTime Posted { get; set; }
	}
}
