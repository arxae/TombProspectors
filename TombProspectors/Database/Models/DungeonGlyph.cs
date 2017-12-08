namespace TombProspectors.Database.Models
{
	using LinqToDB.Mapping;

	[Table(Name = "DungeonGlyphs")]
	public class DungeonGlyph
	{
		[PrimaryKey, NotNull]
		public string Glyph { get; set; }

		[Column, NotNull]
		public string RootChalice { get; set; }

		[Column]
		public bool Fetid { get; set; }

		[Column]
		public bool Rotted { get; set; }

		[Column]
		public bool Cursed { get; set; }

		[Column]
		public string Boss { get; set; }

		[Column]
		public string Loot { get; set; }

		[Column]
		public string Notes { get; set; }

		[Column, NotNull]
		public System.DateTime Updated { get; set; }
	}
}
