
namespace TombProspectors.Controllers
{
	using System.Collections.Generic;
	using System.Linq;

	using Microsoft.AspNetCore.Mvc;

	using Database;
	using Database.Models;
	using Models;

	public class ApiController : Controller
	{
		[HttpGet]
		public string Index() => "YAY :D";

		// Get stuff
		[HttpGet]
		public IEnumerable<DungeonBoss> Bosses()
		{
			using (var db = new ChaliceDb())
			{
				return db.DungeonBosses.ToList();
			}
		}

		// TODO: Return paged
		[HttpGet]
		public IEnumerable<DungeonGlyph> Glyphs()
		{
			using (var db = new ChaliceDb())
			{
				return db.DungeonGlyphs.ToList();
			}
		}

		[HttpGet]
		public IEnumerable<RootChalice> RootChalices()
		{
			using (var db = new ChaliceDb())
			{
				return db.RootChalices.ToList();
			}
		}

		// Post stuff
		[HttpPost]
		public ActionResult SubmitNewGlyph(SubmitNewGlyphModel newGlyph)
		{
			string res = ChaliceDb.NewGlyphFromModel(newGlyph);

			if (res == "<EXISTS>")
			{
				return View("_Error", "This glyph already exists");
			}

			return Redirect($"/dungeon/viewglyph/{res}");
		}
	}
}
