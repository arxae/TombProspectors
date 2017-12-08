namespace TombProspectors.Database.Models
{
	using LinqToDB.Mapping;

	[Table(Name = "Bosses")]
	public class DungeonBoss
	{
		[PrimaryKey, Identity]
		public int Id { get; set; }

		[Column, NotNull]
		public string BossName { get; set; }

		[Column]
		public string WikiLink { get; set; }
	}
}
