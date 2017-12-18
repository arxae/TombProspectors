namespace TombProspectors.Database
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using LinqToDB;
	using LinqToDB.Data;

	using Controllers.Models;
	using Models;

	public class ChaliceDb : DataConnection
	{
		public ITable<DungeonGlyph> DungeonGlyphs => GetTable<DungeonGlyph>();
		public ITable<RootChalice> RootChalices => GetTable<RootChalice>();
		public ITable<DungeonBoss> DungeonBosses => GetTable<DungeonBoss>();
		public ITable<Loot> Loot => GetTable<Loot>();
		public ITable<User> Users => GetTable<User>();
		public ITable<UserHistory> UserHistory => GetTable<UserHistory>();
		public ITable<Comment> Comments => GetTable<Comment>();
		public ITable<Article> Articles => GetTable<Article>();

		public static string NewGlyphFromModel(SubmitGlyphModel glyph)
		{
			var hs = new Ganss.XSS.HtmlSanitizer();

			var nGlyph = new DungeonGlyph
			{
				Glyph = hs.Sanitize(glyph.DungeonGlyph),
				ShortDescription = hs.Sanitize(glyph.ShortDescription),
				Layers = glyph.Layers,
				RootChalice = glyph.RootChalice,
				Fetid = glyph.Fetid,
				Rotted = glyph.Rotted,
				Cursed = glyph.Cursed,
				Sinister = glyph.Sinister,
				SaveEdited = glyph.SaveEdited,
				Bosses = $";{string.Join(';', glyph.Bosses)};",
				Notes = hs.Sanitize(glyph.Notes),
				Submitter = glyph.Submitter,
				Upvotes = 1,
				Downvotes = 0,
				Updated = DateTime.Now
			};

			using (var db = new ChaliceDb())
			{
				// Check if glyph exists
				if (db.DungeonGlyphs.Any(g => g.Glyph == nGlyph.Glyph)) return "<EXISTS>";

				nGlyph.Loot = $";{VerifyLootList(glyph.Loot)};";

				db.Insert(nGlyph);

				db.InsertWithIdentity(new UserHistory
				{
					UserName = nGlyph.Submitter,
					Target = nGlyph.Glyph,
					Action = "new_glyph",
					Value = nGlyph.Glyph,
					Created = DateTime.Now
				});

				return nGlyph.Glyph;
			}
		}

		internal static bool UpdateGlyphFromModel(SubmitGlyphModel glp)
		{
			var hs = new Ganss.XSS.HtmlSanitizer();

			using (var db = new ChaliceDb())
			{
				db.DungeonGlyphs
					   .Where(d => d.Glyph == glp.DungeonGlyph)
					   .Set(d => d.RootChalice, hs.Sanitize(glp.RootChalice))
					   .Set(d => d.Fetid, glp.Fetid)
					   .Set(d => d.Rotted, glp.Rotted)
					   .Set(d => d.Cursed, glp.Cursed)
					   .Set(d => d.Sinister, glp.Sinister)
					   .Set(d => d.SaveEdited, glp.SaveEdited)
					   .Set(d => d.Bosses, $";{string.Join(';', glp.Bosses)};")
					   .Set(d => d.Loot, $";{VerifyLootList(glp.Loot)};")
					   .Set(d => d.Notes, hs.Sanitize(glp.Notes))
					   .Set(d => d.Updated, DateTime.Now)
					   .Update();
			}

			return true;
		}

		/// <summary>
		/// Checks if loot exists, if not, create an entry for it
		/// </summary>
		/// <param name="lootIds">A list of numbers, if a string is passed, then create a new entry for it</param>
		/// <returns>A string containing only loot ids</returns>
		public static string VerifyLootList(string[] lootIds)
		{
			using (var db = new ChaliceDb())
			{
				var lootList = new List<int>();
				foreach (var loot in lootIds)
				{
					if (int.TryParse(loot, out int itemId) == false)
					{
						var lootEntry = new Loot
						{
							ItemName = loot,
							Updated = DateTime.Now
						};

						lootList.Add(Convert.ToInt32(db.InsertWithIdentity(lootEntry)));
					}
					else
					{
						var entry = db.Loot.FirstOrDefault(l => l.Id == int.Parse(loot));

						// If it doesn't, add entry
						if (entry == null)
						{
							// Shouldn't happen, but if it happens often, consider checking/adding here again
							throw new Exception("Item should be added to db");
						}

						lootList.Add(entry.Id);
					}
				}

				return string.Join(';', lootList);
			}
		}

		public static bool RegisterNewUser(NewUserRegistrationModel registration)
		{
			var user = new User
			{
				UserName = new Ganss.XSS.HtmlSanitizer().Sanitize(registration.UserName),
				Password = Security.GetPasswordHashString(registration.Password),
				Email = registration.Email,
				Role = "user",
				Created = DateTime.Now
			};

			var history = new UserHistory
			{
				UserName = user.UserName,
				Target = "account",
				Action = "created",
				Value = user.UserName,
				Created = DateTime.Now
			};

			using (var db = new ChaliceDb())
			{
				var userEntry = db.Users.FirstOrDefault(d => d.UserName == user.UserName || d.Email == user.Email);

				if (userEntry != null)
				{
					return false;
				}

				db.BeginTransaction();

				db.InsertWithIdentity(user);
				db.InsertWithIdentity(history);

				db.CommitTransaction();
			}

			return true;
		}

		public static GeneralSearchModel SearchGlyph(string query, string type)
		{
			using (var db = new ChaliceDb())
			{
				var glyphResults = new List<SearchResultEntry>();
				var lootResults = new List<Loot>();

				// If it's a directly matching glyph, just return it immediately instead of searching on
				var entry = db.DungeonGlyphs.FirstOrDefault(d => d.Glyph == query);
				if (entry != null)
				{
					return GeneralSearchModel.FromSingleEntry(new SearchResultEntry
					{
						Glyph = entry.Glyph,
						ShortDescription = entry.ShortDescription,
						Submitter = entry.Submitter,
						Updated = entry.Updated
					});

				}

				if (type == "glyph")
				{
					// Search for semi-matching glyphs
					var glyphs = db.DungeonGlyphs.Where(d => d.Glyph.Contains(query)).ToList();
					if (glyphs.Count == 0) return new GeneralSearchModel();
					foreach (var g in glyphs)
					{
						if (glyphResults.Any(x => x.Glyph == g.Glyph)) continue;
						glyphResults.Add(new SearchResultEntry
						{
							Glyph = g.Glyph,
							ShortDescription = g.ShortDescription,
							RootChalice = db.RootChalices.FirstOrDefault(r => r.ChaliceId == g.RootChalice).ChaliceName,
							Submitter = g.Submitter,
							Upvotes = g.Upvotes,
							Downvotes = g.Downvotes,
							Updated = g.Updated
						});
					}
				}
				else if (type == "loot")
				{
					// Get loot ids
					var lootEntries = db.Loot.Where(l => l.ItemName.Contains(query)).ToList();

					if (lootEntries.Any() == false) return new GeneralSearchModel();

					// Check glyphs for matching ids
					foreach (var loot in lootEntries)
					{
						var glyphs = db
							.Query<DungeonGlyph>(
								$"SELECT Glyph, ShortDescription, RootChalice, Submitter, Updated FROM DungeonGlyphs WHERE(',' + RTRIM(Loot) + ';') LIKE '%;{loot.Id};%'")
							.ToList();

						if (glyphs.Count == 0) continue;
						foreach (var g in glyphs)
						{
							if (lootResults.Any(l => l.Id == loot.Id) == false)
							{
								lootResults.Add(db.Loot.FirstOrDefault(l => l.Id == loot.Id));
							}

							if (glyphResults.Any(x => x.Glyph == g.Glyph)) continue;
							glyphResults.Add(new SearchResultEntry
							{
								Glyph = g.Glyph,
								ShortDescription = g.ShortDescription,
								RootChalice = db.RootChalices.FirstOrDefault(r => r.ChaliceId == g.RootChalice).ChaliceName,
								Submitter = g.Submitter,
								Upvotes = g.Upvotes,
								Downvotes = g.Downvotes,
								Updated = g.Updated
							});
						}
					}
				}

				return new GeneralSearchModel(glyphResults, lootResults);
			}
		}

		public static User GetUserByUsername(string username)
		{
			using (var db = new ChaliceDb())
			{
				return db.Users.FirstOrDefault(u => u.UserName == username);
			}
		}

		public static IEnumerable<UserHistory> GetUserHistory(string userName)
		{
			using (var db = new ChaliceDb())
			{
				return db.UserHistory
					.Where(u => u.UserName == userName)
					.OrderBy(u => u.Created)
					.ToList();
			}
		}

		/// <summary>
		/// Recounts the amount of up and downvotes for a glyph and updates the data
		/// </summary>
		/// <param name="glyph"></param>
		public static void RebuildGlyphVotes(string glyph)
		{
			using (var db = new ChaliceDb())
			{
				var result = db.Query<int>($"WITH Votes (Up, Down) AS (SELECT  COALESCE(SUM(CASE WHEN [Value] = 'up' THEN 1 ELSE 0 END), 1) AS Up, COALESCE(SUM(CASE WHEN [Value] = 'down' THEN 1 ELSE 0 END), 0) AS Down FROM UserHistory WHERE [Target] = '{glyph}' AND [Action] = 'vote') UPDATE DungeonGlyphs SET Upvotes = (SELECT Up FROM Votes), Downvotes = (SELECT Down FROM Votes) WHERE Glyph = '{glyph}'");
			}
		}

		public static void RemoveVote(UserHistory voteEntry)
		{
			using (var db = new ChaliceDb())
			{
				db.BeginTransaction();
				db.Delete(voteEntry);
				db.CommitTransaction();
			}
		}
	}
}
