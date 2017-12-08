namespace TombProspectors.Database.Models
{
	using LinqToDB.Mapping;

	[Table(Name = "Loot")]
	public class Loot
	{
		[PrimaryKey, Identity]
		public int Id { get; set; }

		[Column, NotNull]
		public string ItemName { get; set; }

		[Column]
		public string Note { get; set; }

		[Column]
		public string WikiLink { get; set; }

		[Column, NotNull]
		public System.DateTime Updated { get; set; }
	}
}
