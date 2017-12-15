namespace TombProspectors.Controllers
{
	using System;
	using System.Linq;

	using Database;

	using Microsoft.AspNetCore.Mvc;

	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			var model = new HomeViewStats();
			using (var db = new ChaliceDb())
			{
				// TODO: Change to created instead of updated
				model.LatestGlyph = db.DungeonGlyphs
					.OrderByDescending(d => d.Updated)
					.FirstOrDefault()
					?.Glyph;

				var mostSubmittedQuery = (from entry in db.DungeonGlyphs
										  group entry by entry.Submitter
						into entries
										  let count = entries.Count()
										  orderby count descending
										  select new { Submitter = entries.Key, Count = count })
					.FirstOrDefault();

				if (mostSubmittedQuery != null)
				{
					model.MostSubmissions = new Tuple<string, int>(mostSubmittedQuery.Submitter, mostSubmittedQuery.Count);
				}
			}

			return View(model);
		}

		public class HomeViewStats
		{
			public string LatestGlyph { get; set; }
			public Tuple<string, int> MostSubmissions { get; set; }
		}
	}
}
