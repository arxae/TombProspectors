namespace TombProspectors.Controllers.Models
{
	using System.Collections.Generic;
	using Microsoft.AspNetCore.Http;

	public class SubmitGlyphModel
	{
		public string DungeonGlyph { get; set; }
		public string ShortDescription { get; set; }
		public string RootChalice { get; set; }
		public int Layers { get; set; }
		public bool Fetid { get; set; }
		public bool Rotted { get; set; }
		public bool Cursed { get; set; }
		public bool Sinister { get; set; }
		public bool SaveEdited { get; set; }
		public int[] Bosses { get; set; }
		public string Notes { get; set; }
		public string[] Loot { get; set; }
		public string Submitter { get; set; }
		public string DeletedImages { get; set; }
		public List<IFormFile> Screenshots { get; set; }
	}
}