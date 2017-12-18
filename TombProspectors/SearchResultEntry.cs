namespace TombProspectors
{
	using System;

	public class SearchResultEntry
	{
		public string Glyph { get; set; }
		public string ShortDescription { get; set; }
		public string RootChalice { get; set; }
		public string Submitter { get; set; }
		public int Upvotes { get; set; }
		public int Downvotes { get; set; }
		public int Closedvotes { get; set; }
		public DateTime Updated { get; set; }
	}
}
