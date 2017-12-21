namespace TombProspectors.Controllers
{
	using System.Collections.Generic;
	using System.Linq;

	using LinqToDB.Data;
	using Microsoft.AspNetCore.Mvc;

	using Database;
	using Database.Models;

	public class ListsController : Controller
	{
		[HttpGet]
		public IActionResult GlyphsByLoot()
		{
			List<Loot> model = new List<Loot>();
			using (var db = new ChaliceDb())
			{
				model.AddRange(db.Loot.ToList());
			}

			return View(model);
		}

		[HttpGet]
		public IActionResult LoadGlyphsByLoot(string lootId)
		{
			var resultList = new List<SearchResultEntry>();
			using (var db = new ChaliceDb())
			{
				var glyphs = db
					.Query<DungeonGlyph>(
						$"SELECT Glyph, ShortDescription, RootChalice, Submitter, Upvotes, Downvotes, Closedvotes, Updated FROM DungeonGlyphs WHERE(',' + RTRIM(Loot) + ';') LIKE '%;{lootId};%'")
					.ToList();

				foreach (var g in glyphs)
				{
					if (resultList.Any(x => x.Glyph == g.Glyph)) continue;
					resultList.Add(new SearchResultEntry
					{
						Glyph = g.Glyph,
						ShortDescription = g.ShortDescription,
						RootChalice = db.RootChalices.FirstOrDefault(r => r.ChaliceId == g.RootChalice).ChaliceName,
						Submitter = g.Submitter,
						Upvotes = g.Upvotes,
						Downvotes = g.Downvotes,
						Closedvotes = g.ClosedVotes,
						Updated = g.Updated
					});
				}
			}

			return PartialView("Lists/_GlyphsResultView", resultList);
		}

		[HttpGet]
		public IActionResult GlyphsByBoss()
		{
			var model = new List<DungeonBoss>();
			using (var db = new ChaliceDb())
			{
				model.AddRange(db.DungeonBosses.ToList());
			}

			return View(model);
		}

		public IActionResult LoadGlyphsByBoss(string bossId)
		{
			var resultList = new List<SearchResultEntry>();

			using (var db = new ChaliceDb())
			{
				var glyphs = db
					.Query<DungeonGlyph>(
						$"SELECT Glyph, ShortDescription, RootChalice, Submitter, Upvotes, Downvotes, Closedvotes Updated FROM DungeonGlyphs WHERE(',' + RTRIM(Bosses) + ';') LIKE '%;{bossId};%'")
					.ToList();

				foreach (var g in glyphs)
				{
					if (resultList.Any(x => x.Glyph == g.Glyph)) continue;
					resultList.Add(new SearchResultEntry
					{
						Glyph = g.Glyph,
						ShortDescription = g.ShortDescription,
						RootChalice = db.RootChalices.FirstOrDefault(r => r.ChaliceId == g.RootChalice).ChaliceName,
						Submitter = g.Submitter,
						Upvotes = g.Upvotes,
						Downvotes = g.Downvotes,
						Closedvotes = g.ClosedVotes,
						Updated = g.Updated
					});
				}
			}

			return PartialView("Lists/_GlyphsResultView", resultList);
		}

		[HttpGet]
		public IActionResult GlyphsByRootChalices()
		{
			var model = new List<RootChalice>();
			using (var db = new ChaliceDb())
			{
				model.AddRange(db.RootChalices.ToList());
			}

			return View(model);
		}

		[HttpGet]
		public IActionResult LoadGlyphsByRootChalice(string chaliceId, string rites)
		{
			var resultList = new List<SearchResultEntry>();
			using (var db = new ChaliceDb())
			{
				var glyphsQuery = db.DungeonGlyphs.Where(d => d.RootChalice == chaliceId);

				if (rites != "all")
				{
					string[] ritesArr = rites.Split(",", System.StringSplitOptions.RemoveEmptyEntries);
					foreach (var r in ritesArr)
					{
						switch (r)
						{
							case "fetid": glyphsQuery = glyphsQuery.Where(g => g.Fetid); break;
							case "rotted": glyphsQuery = glyphsQuery.Where(g => g.Rotted); break;
							case "cursed": glyphsQuery = glyphsQuery.Where(g => g.Cursed); break;
							case "sinister": glyphsQuery = glyphsQuery.Where(g => g.Sinister); break;
							case "saveedit": glyphsQuery = glyphsQuery.Where(g => g.SaveEdited); break;
						}
					}
				}

				var glyphs = glyphsQuery.ToList();

				foreach (var g in glyphs)
				{
					if (resultList.Any(x => x.Glyph == g.Glyph)) continue;
					resultList.Add(new SearchResultEntry
					{
						Glyph = g.Glyph,
						ShortDescription = g.ShortDescription,
						RootChalice = db.RootChalices.FirstOrDefault(r => r.ChaliceId == g.RootChalice).ChaliceName,
						Submitter = g.Submitter,
						Upvotes = g.Upvotes,
						Downvotes = g.Downvotes,
						Closedvotes = g.ClosedVotes,
						Updated = g.Updated
					});
				}
			}

			return PartialView("Lists/_GlyphsResultView", resultList);
		}
	}
}
