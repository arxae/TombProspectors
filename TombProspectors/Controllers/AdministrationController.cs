namespace TombProspectors.Controllers
{
	using System.Linq;

	using LinqToDB;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	using Database;
	using Database.Models;

	public class AdministrationController : Controller
	{
		[Authorize]
		[HttpGet]
		public IActionResult NewHomepageItem()
		{
			if (UserHasAdminRoles() == false) return View("_Error", "You are not authorized to do this");

			return View();
		}

		[Authorize]
		[HttpPost]
		public IActionResult PostNewHomepageItem([FromForm] HomePageItem item)
		{
			if (UserHasAdminRoles() == false) return View("_Error", "You are not authorized to do this");

			using (var db = new ChaliceDb())
			{
				db.BeginTransaction();

				db.InsertWithIdentity(new Article
				{
					Section = "homepage",
					Title = item.Title,
					Content = item.Content,
					PostedBy = User.Identity.Name,
					Posted = System.DateTime.Now
				});

				db.InsertWithIdentity(new UserHistory
				{
					UserName = User.Identity.Name,
					Action = "new_article",
					Target = "homepage",
					Value = item.Title,
					Created = System.DateTime.Now
				});

				db.CommitTransaction();
			}

			return Redirect("/");
		}

		[Authorize]
		[HttpPost]
		public IActionResult DeleteGlyph([FromBody] string glyphId)
		{
			if (UserHasAdminRoles() == false) return View("_Error", "You are not authorized to do this");

			// Delete glyph and associated history items (created + votes)
			using (var db = new ChaliceDb())
			{
				db.BeginTransaction();

				db.DungeonGlyphs.Delete(d => d.Glyph == glyphId);
				db.UserHistory.Delete(h => h.Target == glyphId);

				db.CommitTransaction();
			}

			return Ok("deleted");
		}

		[Authorize]
		[HttpPost]
		public IActionResult RebuildGlyphVotes([FromBody] string glyphId)
		{
			if (UserHasAdminRoles() == false) return View("_Error", "You are not authorized to do this");

			ChaliceDb.RebuildGlyphVotes(glyphId);

			return Ok("rebuild");
		}

		[Authorize]
		[HttpGet]
		public IActionResult UserManagement()
		{
			using (var db = new ChaliceDb())
			{
				var model = db.Users.ToList();

				return View(model);
			}
		}

		[Authorize]
		[HttpPost]
		public IActionResult DeleteUser([FromBody] int userId)
		{
			if (UserHasAdminRoles() == false) return View("_Error", "You are not authorized to do this");

			using (var db = new ChaliceDb())
			{
				db.BeginTransaction();

				var user = db.Users.FirstOrDefault(u => u.Id == userId);

				if(user == null)
				{
					return NotFound("user not found");
				}
				
				var glyphs = db.DungeonGlyphs.Where(g => g.Submitter == user.UserName).ToList();
				if(glyphs.Count > 0)
				{
					foreach(var g in glyphs)
					{
						g.Submitter = "[RemovedUser]";
						db.Update(g);
					}
				}

				db.Users.Delete(u => u.Id == userId);
				db.UserHistory.Delete(h => h.UserName == user.UserName);

				db.CommitTransaction();
			}

			return Ok("user deleted");
		}

		public bool UserHasAdminRoles()
		{
			return User.HasClaim(c => c.Value == "admin" || c.Value == "mod");
		}

		public class HomePageItem
		{
			public string Title { get; set; }
			public string Content { get; set; }
		}
	}
}
