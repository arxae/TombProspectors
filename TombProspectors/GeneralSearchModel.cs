namespace TombProspectors
{
	using System.Collections.Generic;

	using Database.Models;

	public class GeneralSearchModel
	{
		public List<SearchResultEntry> GlyphResults { get; set; }
		public List<Loot> LootResults { get; set; }

		public GeneralSearchModel()
		{
			GlyphResults = new List<SearchResultEntry>();
			LootResults = new List<Loot>();
		}

		public GeneralSearchModel(List<SearchResultEntry> glyphResults, List<Loot> lootResults)
		{
			GlyphResults = glyphResults;
			LootResults = lootResults;
		}

		public static GeneralSearchModel FromSingleEntry(SearchResultEntry entry)
		{
			var m = new GeneralSearchModel();
			m.GlyphResults.Add(entry);

			return m;
		}
	}
}
