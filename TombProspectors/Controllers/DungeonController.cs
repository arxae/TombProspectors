﻿using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;

namespace TombProspectors.Controllers
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	using LinqToDB;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	using Database;
	using Database.Models;
	using Models;

	using FileIo = System.IO.File;

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

				string ssDirPath = Path.Combine("wwwroot", "Screenshots", glyph);
				if (Directory.Exists(ssDirPath))
				{
					string[] files = Directory.GetFiles(ssDirPath);

					foreach (var file in files)
					{
						var fileName = Path.GetFileName(file);
						model.Screenshots.Add(fileName);
					}
				}
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

			string ssDirPath = Path.Combine("wwwroot", "Screenshots", glyph);
			if (Directory.Exists(ssDirPath))
			{
				string[] files = Directory.GetFiles(ssDirPath);

				foreach (var file in files)
				{
					var fileName = Path.GetFileName(file);
					model.Screenshots.Add(fileName);
				}
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

			if (glyph.Screenshots != null && glyph.Screenshots.Count > 0)
			{
				string[] validFileTypes = { "image/jpeg", "image/gif", "image/png" };
				foreach (var file in glyph.Screenshots)
				{
					if (file.Length < 0) continue;
					if (validFileTypes.Contains(file.ContentType) == false) continue;

					SaveImage(glyph.DungeonGlyph, file);
				}
			}

			string res = ChaliceDb.NewGlyphFromModel(glyph);

			if (res == "<EXISTS>")
			{
				return View("_Error", "This glyph alreadt exists");
			}

			return Redirect($"/dungeon/viewglyph/{res}");
		}

		[HttpPost]
		public IActionResult SubmitGlyphEdit(SubmitGlyphModel glp)
		{
			var sanitizer = new Ganss.XSS.HtmlSanitizer();
			glp.Submitter = HttpContext.User.Identity.Name;
			glp.Notes = sanitizer.Sanitize(glp.Notes);

			string ssDirPath = Path.Combine("wwwroot", "Screenshots", glp.DungeonGlyph);
			if (glp.DeletedImages != null)
			{
				var ids = glp.DeletedImages.Split(';');
				foreach (var id in ids)
				{
					var mainFile = Path.Combine(ssDirPath, $"{id}.png");
					var thumbFile = Path.Combine(ssDirPath, $"{id}-thumb.png");

					if (FileIo.Exists(mainFile)) FileIo.Delete(mainFile);
					if (FileIo.Exists(thumbFile)) FileIo.Delete(thumbFile);
				}
			}

			if (glp.Screenshots != null && glp.Screenshots.Count > 0)
			{
				string[] validFileTypes = { "image/jpeg", "image/gif", "image/png" };
				foreach (var file in glp.Screenshots)
				{
					if (file.Length <= 0) continue;
					if (validFileTypes.Contains(file.ContentType) == false) continue; // Skip when file is not a supported imagetype

					SaveImage(glp.DungeonGlyph, file);
				}
			}

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
				switch (vote.Vote.ToLower())
				{
					case "up":
						glyph.Upvotes += 1;
						break;
					case "down":
						glyph.Downvotes += 1;
						break;
					case "retract":
						{
							var prevVote = db.UserHistory.FirstOrDefault(h => h.UserName == User.Identity.Name && h.Target == vote.Glyph && h.Action == "vote");
							switch (prevVote.Value)
							{
								case "up": glyph.Upvotes -= 1; break;
								case "down": glyph.Downvotes -= 1; break;
								case "closed": glyph.ClosedVotes -= 1; break;
							}
							db.Delete(prevVote);
						}
						break;
					case "closed":
						{
							// Delete previous vote if needed
							var prevVote = db.UserHistory.FirstOrDefault(h => h.UserName == User.Identity.Name && h.Target == vote.Glyph && h.Action == "vote");
							if (prevVote != null)
							{
								switch (prevVote.Value)
								{
									case "up": glyph.Upvotes -= 1; break;
									case "down": glyph.Downvotes -= 1; break;
								}
								db.Delete(prevVote);
							}
							glyph.ClosedVotes += 1;
						}
						break;
				}

				db.Update(glyph);

				// Don't write history for vote retraction
				if (vote.Vote.ToLower() != "retract")
				{
					db.InsertWithIdentity(new UserHistory
					{
						UserName = User.Identity.Name,
						Action = "vote",
						Target = vote.Glyph,
						Value = vote.Vote,
						Created = System.DateTime.Now
					});
				}

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

		void SaveImage(string glyph, IFormFile file)
		{
			string ssDirPath = Path.Combine("wwwroot", "Screenshots", glyph);

			if (Directory.Exists(ssDirPath) == false)
			{
				Directory.CreateDirectory(ssDirPath);
			}

			string Id = System.Guid.NewGuid().ToString("N");
			string filePath = Path.Combine(ssDirPath, $"{Id}.png");
			string thumbPath = Path.Combine(ssDirPath, $"{Id}-thumb.png");

			using (var img = Image.Load<Rgba32>(file.OpenReadStream()))
			{
				// Main Image
				using (var output = FileIo.OpenWrite(filePath))
				{
					if (img.Width > 1920 || img.Height > 1080)
					{
						img.Mutate(i => i.Resize(1920, 1080));
					}

					img.SaveAsPng(output);
				}

				// Thumbnail
				using (var output = FileIo.OpenWrite(thumbPath))
				{
					img.Mutate(i => i.Resize(256, 128));
					img.SaveAsPng(output);
				}
			}
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
			public List<string> Screenshots;

			public ViewGlyphModel()
			{
				Bosses = new List<DungeonBoss>();
				Loot = new List<Loot>();
				HasVoted = false;
				Screenshots = new List<string>();
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
