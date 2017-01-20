using Newtonsoft.Json;

namespace BeerTapsConsole
{
	public class BeerTap
	{
		[JsonProperty("Id")]
		public int Id { get; set; }

		[JsonProperty("BeerName")]
		public string BeerName { get; set; }

		[JsonProperty("TotalVolume")]
		public int TotalVolume { get; set; }

		[JsonProperty("CurrentVolume")]
		public int CurrentVolume { get; set; }

		[JsonProperty("KegState")]
		public string KegState { get; set; }

		[JsonProperty("OfficeId")]
		public int OfficeId { get; set; }

		[JsonProperty("_links")]
		public BeerTapLinks Links { get; set; }
	}

	public class BeerTapLinks
	{
		[JsonProperty("iq:Pour")]
		public SingleLink IqPour { get; set; }

		[JsonProperty("iq:ReplaceKeg")]
		public SingleLink IqReplaceKeg { get; set; }

		[JsonProperty("self")]
		public SingleLink Self { get; set; }
	}
}