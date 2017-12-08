namespace TombProspectors.Controllers
{
	using System.Collections.Generic;
	using System.Linq;

	using Microsoft.AspNetCore.Mvc;

	using Database;
	using Database.Models;

	public class DungeonController : Controller
	{
		[HttpGet]
		public IActionResult AddGlyph()
		{
			var model = new AddGlyphModel();

			using (var db = new ChaliceDb())
			{
				model.RootChalices = db.RootChalices.OrderBy(c => c.ChaliceName).ToList();
				model.Bosses = db.DungeonBosses.OrderBy(b => b.BossName).ToList();
				model.Loot = db.Loot.OrderBy(l => l.ItemName).ToList();
			}

			return View(model);
		}

		[HttpGet]
		public IActionResult ViewGlyph(string glyph)
		{
			var model = new ViewGlyphModel();

			using (var db = new ChaliceDb())
			{
				model.Dungeon = db.DungeonGlyphs.FirstOrDefault(g => g.Glyph == glyph);

				foreach (var bossIdString in model.Dungeon.Boss.Split(';'))
				{
					var id = int.Parse(bossIdString);
					var boss = db.DungeonBosses.FirstOrDefault(b => b.Id == id);
					model.Bosses.Add(boss);
				}

				foreach (var lootIdString in model.Dungeon.Loot.Split(';'))
				{
					var id = int.Parse(lootIdString);
					var loot = db.Loot.FirstOrDefault(l => l.Id == id);
					model.Loot.Add(loot);
				}
			}

			return View(model);
		}

		public class AddGlyphModel
		{
			public List<RootChalice> RootChalices;
			public List<DungeonBoss> Bosses;
			public List<Loot> Loot;
		}

		public class ViewGlyphModel
		{
			public DungeonGlyph Dungeon;
			public List<DungeonBoss> Bosses;
			public List<Loot> Loot;

			public ViewGlyphModel()
			{
				Bosses = new List<DungeonBoss>();
				Loot = new List<Loot>();
			}
		}
	}
}
