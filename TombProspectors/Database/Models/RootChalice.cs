namespace TombProspectors.Database.Models
{
	using LinqToDB.Mapping;

	[Table(Name = "RootChalices")]
	public class RootChalice
	{
		[PrimaryKey, NotNull]
		public string ChaliceId { get; set; }

		[Column, NotNull]
		public string ChaliceName { get; set; }

		public override string ToString() => $"{ChaliceName} ({ChaliceId})";
	}
}
