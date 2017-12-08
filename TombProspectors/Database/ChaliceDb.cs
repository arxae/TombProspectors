namespace TombProspectors.Database
{
	using System.Collections.Generic;
	using System.Linq;

	using LinqToDB;
	using LinqToDB.Data;

	using Controllers.Models;
	using Models;

	public class ChaliceDb : DataConnection
	{
		public ITable<DungeonGlyph> DungeonGlyphs => GetTable<DungeonGlyph>();
		public ITable<RootChalice> RootChalices => GetTable<RootChalice>();
		public ITable<DungeonBoss> DungeonBosses => GetTable<DungeonBoss>();
		public ITable<Loot> Loot => GetTable<Loot>();

		public static string NewGlyphFromModel(SubmitNewGlyphModel newGlyph)
		{
			var nGlyph = new DungeonGlyph
			{
				Glyph = newGlyph.DungeonGlyph,
				Boss = string.Join(';', newGlyph.Bosses),
				Fetid = newGlyph.Fetid,
				Rotted = newGlyph.Rotted,
				Cursed = newGlyph.Cursed,
				Notes = newGlyph.Notes,
				RootChalice = newGlyph.RootChalice,
				Updated = System.DateTime.Now
			};

			using (var db = new ChaliceDb())
			{
				// Check if glyph exists
				if (db.DungeonGlyphs.Any(g => g.Glyph == nGlyph.Glyph)) return "<EXISTS>";

				// Check if loot exists
				var lootList = new List<int>();
				foreach (var loot in newGlyph.Loot)
				{
					var entry = db.Loot.FirstOrDefault(l => l.ItemName == loot);

					// If it doesn't, add entry
					if (entry == null)
					{
						var lootEntry = new Loot
						{
							ItemName = loot,
							Updated = System.DateTime.Now
						};


						lootList.Add(System.Convert.ToInt32(db.InsertWithIdentity(lootEntry)));
					}
				}

				nGlyph.Loot = string.Join(';', lootList);

				db.Insert(nGlyph);

				return nGlyph.Glyph;
			}
		}
	}
}
