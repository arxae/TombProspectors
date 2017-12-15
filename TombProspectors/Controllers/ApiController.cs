namespace TombProspectors.Controllers
{
	using System.Collections.Generic;
	using System.Linq;

	using Microsoft.AspNetCore.Mvc;

	using Database;
	using Database.Models;

	[Route("[controller]")]
	public class ApiController : Controller
	{
		[HttpGet]
		public string Index() => "YAY :D";

		[HttpGet("boss")]
		public IEnumerable<int> AllBosses()
		{
			using (var db = new ChaliceDb())
			{
				return db.DungeonBosses.Select(b => b.Id).ToList();
			}
		}

		[HttpGet("boss/{id}")]
		public DungeonBoss BossById(int id)
		{
			using (var db = new ChaliceDb())
			{
				return db.DungeonBosses.FirstOrDefault(b => b.Id == id);
			}
		}

		[HttpGet("chalice")]
		public IEnumerable<string> AllChalices()
		{
			using (var db = new ChaliceDb())
			{
				return db.RootChalices.Select(c => c.ChaliceId).ToList();
			}
		}

		[HttpGet("chalice/{id}")]
		public RootChalice ChaliceById(string id)
		{
			using (var db = new ChaliceDb())
			{
				return db.RootChalices.FirstOrDefault(b => b.ChaliceId == id);
			}
		}

		[HttpGet("loot")]
		public IEnumerable<int> AllLoot()
		{
			using (var db = new ChaliceDb())
			{
				return db.Loot.Select(l => l.Id).ToList();
			}
		}

		[HttpGet("loot/{id}")]
		public Loot LootById(int id)
		{
			using (var db = new ChaliceDb())
			{
				return db.Loot.FirstOrDefault(l => l.Id == id);
			}
		}

		// TODO: Return paged
		[HttpGet("glyph")]
		public IEnumerable<string> AllGlyphIds()
		{
			using (var db = new ChaliceDb())
			{
				return db.DungeonGlyphs.Select(g => g.Glyph).ToList();
			}
		}

		[HttpGet("glyph/{id}")]
		public DungeonGlyph GlyphById(string id)
		{
			using (var db = new ChaliceDb())
			{
				return db.DungeonGlyphs.FirstOrDefault(g => g.Glyph == id);
			}
		}

		//[HttpGet("search/{statement}")]
		//public IEnumerable<DungeonGlyph> GeneralSearchByStatement(string statement)
		//{
		//	using (var db = new ChaliceDb())
		//	{
		//		// Gather ids to search through
		//		var bossIds = db.DungeonBosses.Where(b => b.BossName.Contains(statement)).Select(b => b.Id).ToList();
		//		var lootIds = db.Loot.Where(l => l.ItemName.Contains(statement)).Select(l => l.Id).ToList();
		//		var chaliceIds = db.RootChalices.Where(c => c.ChaliceName.Contains(statement)).Select(c => c.ChaliceId).ToList();

		//		var dungeons = new List<DungeonGlyph>();

		//		foreach (var bossId in bossIds)
		//		{
		//			dungeons.AddRange(db.DungeonGlyphs.Where(d => d.Boss.Contains(bossId.ToString())));
		//		}

		//		foreach (var lootId in lootIds)
		//		{
		//			dungeons.AddRange(db.DungeonGlyphs.Where(d => d.Loot.Contains(lootId.ToString())));
		//		}

		//		foreach (var chaliceId in chaliceIds)
		//		{
		//			dungeons.AddRange(db.DungeonGlyphs.Where(d => d.RootChalice.Contains(chaliceId)));
		//		}

		//		return dungeons.Distinct();
		//	}
		//}
	}
}
