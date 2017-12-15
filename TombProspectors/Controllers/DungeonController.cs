namespace TombProspectors.Controllers
{
	using System.Collections.Generic;
	using System.Linq;

	using LinqToDB;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	using Database;
	using Database.Models;
	using Models;

	public class DungeonController : Controller
	{
		[Authorize]
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

				if (model.Dungeon == null)
				{
					return View("_Error", "Glyph not found");
				}

				foreach (var bossIdString in model.Dungeon.Bosses.Split(';', System.StringSplitOptions.RemoveEmptyEntries))
				{
					var id = int.Parse(bossIdString);
					var boss = db.DungeonBosses.FirstOrDefault(b => b.Id == id);
					model.Bosses.Add(boss);
				}

				foreach (var lootIdString in model.Dungeon.Loot.Split(';', System.StringSplitOptions.RemoveEmptyEntries))
				{
					var id = int.Parse(lootIdString);
					var loot = db.Loot.FirstOrDefault(l => l.Id == id);
					model.Loot.Add(loot);
				}

				model.RootChaliceDisplayName = db.RootChalices.FirstOrDefault(c => c.ChaliceId == model.Dungeon.RootChalice).ChaliceName;

				var usrname = HttpContext.User.Identity.Name;
				model.HasVoted = db.UserHistory.Any(h => h.UserName == usrname && h.Action == "vote" && h.Target == glyph);
			}

			ViewBag.CurrentGlyph = model.Dungeon.Glyph;
			return View(model);
		}

		public IActionResult LoadComments(string glyph)
		{
			var model = new CommentsViewModel
			{
				Glyph = glyph
			};

			using (var db = new ChaliceDb())
			{
				model.Comments.AddRange(db.Comments.Where(c => c.Glyph == glyph).OrderByDescending(c => c.Posted).ToList());
			}

			return PartialView("_CommentList", model);
		}

		[HttpGet]
		public IActionResult Edit(string glyph)
		{
			var model = new EditGlyphModel();

			using (var db = new ChaliceDb())
			{
				model.Dungeon = db.DungeonGlyphs.FirstOrDefault(g => g.Glyph == glyph);

				if (model.Dungeon == null)
				{
					return View("_Error", "Glyph not found");
				}

				foreach (var bossIdString in model.Dungeon.Bosses.Split(';', System.StringSplitOptions.RemoveEmptyEntries))
				{
					var id = int.Parse(bossIdString);
					var boss = db.DungeonBosses.FirstOrDefault(b => b.Id == id);
					model.Bosses.Add(boss);
				}

				foreach (var lootIdString in model.Dungeon.Loot.Split(';', System.StringSplitOptions.RemoveEmptyEntries))
				{
					var id = int.Parse(lootIdString);
					var loot = db.Loot.FirstOrDefault(l => l.Id == id);
					model.Loot.Add(loot);
				}

				model.RootChaliceDisplayName = db.RootChalices.FirstOrDefault(c => c.ChaliceId == model.Dungeon.RootChalice).ChaliceName;

				model.Lists.RootChalices = db.RootChalices.OrderBy(c => c.ChaliceName).ToList();
				model.Lists.Bosses = db.DungeonBosses.OrderBy(b => b.BossName).ToList();
				model.Lists.Loot = db.Loot.OrderBy(l => l.ItemName).ToList();
			}

			ViewBag.CurrentGlyph = model.Dungeon.Glyph;

			return View("EditGlyph", model);
		}

		[HttpPost]
		public IActionResult SubmitNewGlyph(SubmitGlyphModel glyph)
		{
			var sanitizer = new Ganss.XSS.HtmlSanitizer();
			glyph.Submitter = HttpContext.User.Identity.Name;
			glyph.Notes = sanitizer.Sanitize(glyph.Notes);

			string res = ChaliceDb.NewGlyphFromModel(glyph);

			if (res == "<EXISTS>")
			{
				return View("_Error", "This glyph already exists");
			}

			return Redirect($"/dungeon/viewglyph/{res}");
		}

		[HttpPost]
		public IActionResult SubmitGlyphEdit(SubmitGlyphModel glp)
		{
			var sanitizer = new Ganss.XSS.HtmlSanitizer();
			glp.Submitter = HttpContext.User.Identity.Name;
			glp.Notes = sanitizer.Sanitize(glp.Notes);

			ChaliceDb.UpdateGlyphFromModel(glp);

			return Redirect($"/dungeon/viewglyph/{glp.DungeonGlyph}");
		}

		[HttpPost]
		public bool SubmitVote([FromBody] VotePackageModel vote)
		{
			using (var db = new ChaliceDb())
			{
				db.BeginTransaction();

				var glyph = db.DungeonGlyphs.FirstOrDefault(d => d.Glyph == vote.Glyph);

				if (string.Equals(vote.Vote, "up", System.StringComparison.OrdinalIgnoreCase))
				{
					glyph.Upvotes += 1;
				}
				else if (string.Equals(vote.Vote, "down", System.StringComparison.OrdinalIgnoreCase))
				{
					glyph.Downvotes += 1;
				}

				db.Update(glyph);

				var user = HttpContext.User.Identity.Name;
				db.InsertWithIdentity(new UserHistory
				{
					UserName = user,
					Action = "vote",
					Target = vote.Glyph,
					Value = vote.Vote,
					Created = System.DateTime.Now
				});

				db.CommitTransaction();
			}

			return true;
		}

		[HttpPost]
		public bool SubmitComment([FromBody] CommentSubmissionModel cm)
		{
			var sanitizer = new Ganss.XSS.HtmlSanitizer();

			using (var db = new ChaliceDb())
			{
				db.BeginTransaction();

				var comment = new Comment
				{
					Glyph = cm.Glyph,
					CommentText = sanitizer.Sanitize(cm.Text),
					PostedBy = User.Identity.Name,
					Posted = System.DateTime.Now
				};

				var history = new UserHistory
				{
					UserName = User.Identity.Name,
					Action = "comment",
					Target = cm.Glyph,
					Value = "",
					Created = System.DateTime.Now
				};

				db.InsertWithIdentity(comment);
				db.InsertWithIdentity(history);

				db.CommitTransaction();
			}

			return true;
		}

		[HttpGet]
		public IActionResult Search()
		{
			var query = Request.Query["q"];
			var searchType = Request.Query["t"];

			var model = ChaliceDb.SearchGlyph(query, searchType);
			ViewBag.SearchQuery = query;
			ViewBag.SearchType = searchType;
			
			return View(model);
		}

		#region Models
		public class VotePackageModel
		{
			public string Vote { get; set; }
			public string Glyph { get; set; }
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
			public string RootChaliceDisplayName;
			public bool HasVoted;

			public ViewGlyphModel()
			{
				Bosses = new List<DungeonBoss>();
				Loot = new List<Loot>();
				HasVoted = false;
			}
		}

		public class EditGlyphModel : ViewGlyphModel
		{
			public EditGlyphModel()
			{
				Lists = new AddGlyphModel();
			}

			public AddGlyphModel Lists;
		}

		public class CommentSubmissionModel
		{
			public string Glyph;
			public string Text;
		}

		public class CommentsViewModel
		{
			public string Glyph;
			public List<Comment> Comments;

			public CommentsViewModel()
			{
				Comments = new List<Comment>();
			}
		}
		#endregion
	}
}
