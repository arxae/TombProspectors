namespace TombProspectors.Controllers
{
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
			if (User.HasClaim(c => c.Value == "admin") == false) return Content("No Permission");

			return View();
		}

		[Authorize]
		[HttpPost]
		public IActionResult PostNewHomepageItem([FromForm] HomePageItem item)
		{
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

		public class HomePageItem
		{
			public string Title { get; set; }
			public string Content { get; set; }
		}
	}
}
