namespace TombProspectors.Database.Models
{
	using System.Collections.Generic;
	using System.Linq;

	using LinqToDB.Mapping;

	[Table(Name = "DungeonGlyphs")]
	public class DungeonGlyph
	{
		[PrimaryKey, NotNull]
		public string Glyph { get; set; }

		[Column, NotNull]
		public string ShortDescription { get; set; }

		[Column, NotNull]
		public int Layers { get; set; }

		[Column, NotNull]
		public string RootChalice { get; set; }

		[Column]
		public bool Fetid { get; set; }

		[Column]
		public bool Rotted { get; set; }

		[Column]
		public bool Cursed { get; set; }

		[Column]
		public bool Sinister { get; set; }

		[Column]
		public bool SaveEdited { get; set; }

		[Column]
		public string Bosses { get; set; }

		[Column]
		public string Loot { get; set; }

		[Column]
		public string Notes { get; set; }

		[Column, NotNull]
		public string Submitter { get; set; }

		[Column, NotNull]
		public int Upvotes { get; set; }

		[Column, NotNull]
		public int Downvotes { get; set; }

		[Column, NotNull]
		public System.DateTime Updated { get; set; }

		public List<DungeonBoss> GetBossObjects()
		{
			var list = new List<DungeonBoss>();
			using (var db = new ChaliceDb())
			{
				foreach (var bossIdString in Bosses.Split(';'))
				{
					var id = int.Parse(bossIdString);
					var boss = db.DungeonBosses.FirstOrDefault(b => b.Id == id);
					list.Add(boss);
				}
			}

			return list;
		}
	}
}
