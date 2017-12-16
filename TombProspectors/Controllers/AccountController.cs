namespace TombProspectors.Controllers
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Security.Claims;
	using System.Threading.Tasks;

	using LinqToDB;
	using Microsoft.AspNetCore.Authentication;
	using Microsoft.AspNetCore.Authentication.Cookies;
	using Microsoft.AspNetCore.Mvc;

	using Database.Models;
	using Models;

	public class AccountController : Controller
	{
		[HttpGet]
		public IActionResult Register()
		{
			if (User.Identity.IsAuthenticated)
			{
				return Redirect("/");
			}

			return View();
		}

		[HttpGet]
		public IActionResult Login()
		{
			var refererUrl = new System.Uri(Request.Headers["Referer"]).PathAndQuery;
			Response.Cookies.Append("tp_returnurl", refererUrl);

			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync();

			return Redirect("/");
		}

		[HttpGet]
		public IActionResult Profile()
		{
			var model = new ProfileModel
			{
				User = Database.ChaliceDb.GetUserByUsername(HttpContext.User.Identity.Name),
				History = Database.ChaliceDb.GetUserHistory(HttpContext.User.Identity.Name)
			};

			using (var db = new Database.ChaliceDb())
			{
				model.User = db.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);

				model.History = db.UserHistory
						.Where(u => u.UserName == HttpContext.User.Identity.Name)
						.OrderBy(u => u.Created)
						.ToList();

				model.SubmittedGlyphs = db.DungeonGlyphs
					.Where(d => d.Submitter == HttpContext.User.Identity.Name)
					.Select(d => new System.Tuple<string, string>(d.Glyph, d.ShortDescription))
					.ToList();
			}

			return View(model);
		}

		[HttpPost]
		public IActionResult New([FromBody] NewUserRegistrationModel newUsr)
		{
			var result = Database.ChaliceDb.RegisterNewUser(newUsr);

			if (result == false)
			{
				return Content("false");
			}

			return Redirect("/account/login");
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginModel login)
		{
			var user = LoginUser(login.UserName, login.Password);
			if (user != null)
			{
				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, user.UserName),
					new Claim("role", user.Role)
				};
				
				var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

				ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
				
				await HttpContext.SignInAsync(
					CookieAuthenticationDefaults.AuthenticationScheme,
					new ClaimsPrincipal(userIdentity),
					new AuthenticationProperties
					{
						IsPersistent = true,
						ExpiresUtc = System.DateTime.UtcNow.AddHours(24) // Expire cookie after 24 hours of inactivity 
					});

				var returnUrl = Request.Cookies["tp_returnurl"];

				// Couldn't find return cookie, return to mainpage
				if (string.IsNullOrWhiteSpace(returnUrl))
				{
					return Redirect("/");
				}

				Response.Cookies.Delete("tp_returnurl");
				Response.Redirect(returnUrl);
			}

			return Content("false");
		}

		private User LoginUser(string uname, string pw)
		{
			using (var db = new Database.ChaliceDb())
			{
				var user = db.Users.FirstOrDefault(u => u.UserName == uname);

				if (user == null) return null;

				if (Security.VerifyUser(user, pw))
				{
					user.LastLogin = System.DateTime.Now;
					db.Update(user);

					return user;
				}
			}

			return null;
		}

		public class LoginModel
		{
			public string UserName { get; set; }
			public string Password { get; set; }
		}

		public class ProfileModel
		{
			public User User { get; set; }
			public IEnumerable<UserHistory> History { get; set; }
			public List<System.Tuple<string, string>> SubmittedGlyphs { get; set; }
		}
	}
}
