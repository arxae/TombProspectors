using TombProspectors.Database.Models;

namespace TombProspectors.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Microsoft.AspNetCore.Mvc;

	using Database;

	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			var model = new HomeViewStats();
			using (var db = new ChaliceDb())
			{
				model.LatestGlyph = db.DungeonGlyphs
					.OrderByDescending(d => d.Updated)
					.FirstOrDefault()
					?.Glyph;

				var mostSubmittedQuery = (from entry in db.DungeonGlyphs
										  group entry by entry.Submitter into entries
										  let count = entries.Count()
										  orderby count descending
										  select new { Submitter = entries.Key, Count = count })
					.FirstOrDefault();

				if (mostSubmittedQuery != null)
				{
					model.MostSubmissions = new Tuple<string, int>(mostSubmittedQuery.Submitter, mostSubmittedQuery.Count);
				}

				model.Articles = db.Articles.OrderByDescending(a => a.Posted).Take(5).ToList();
			}

			ViewBag.TestString = Startup.ConnectionString;

			return View(model);
		}

		[HttpGet("/viewpost/{id}")]
		public IActionResult ViewPost(int id)
		{
			using (var db = new ChaliceDb())
			{
				var article = db.Articles.FirstOrDefault(a => a.Id == id);

				return View(article);}

		}

		public class HomeViewStats
		{
			public string LatestGlyph { get; set; }
			public Tuple<string, int> MostSubmissions { get; set; }
			public List<Article> Articles { get; set; }
		}
	}
}
