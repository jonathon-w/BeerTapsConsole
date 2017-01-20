using Newtonsoft.Json;

namespace BeerTapsConsole
{
	public class Office
	{
		[JsonProperty("Id")]
		public int Id { get; set; }

		[JsonProperty("Location")]
		public string Location { get; set; }

		[JsonProperty("_links")]
		public OfficeLinks Links { get; set; }
	}

	public class OfficeLinks
	{
		[JsonProperty("iq:BeerTaps")]
		public SingleLink IqBeerTaps { get; set; }

		[JsonProperty("self")]
		public SingleLink Self { get; set; }
	}

	public class SingleLink
	{
		[JsonProperty("href")]
		public string Href { get; set; }
	}
}