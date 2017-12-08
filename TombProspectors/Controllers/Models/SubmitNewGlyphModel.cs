namespace TombProspectors.Controllers.Models
{
	public class SubmitNewGlyphModel
	{
		public string DungeonGlyph { get; set; }
		public string RootChalice { get; set; }
		public bool Fetid { get; set; }
		public bool Rotted { get; set; }
		public bool Cursed { get; set; }
		public int[] Bosses { get; set; }
		public string Notes { get; set; }
		public string[] Loot { get; set; }
	}
}