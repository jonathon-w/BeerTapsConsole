using System.Net.Http;
using System.Threading.Tasks;

namespace BeerTapsConsole
{
	public class HttpProtocols
	{
		public static async Task<string> GetResponseAsync(string target)
		{
			using (HttpClient client = new HttpClient())
			using (HttpResponseMessage response = await client.GetAsync(target))
			using (HttpContent content = response.Content)
			{
				return await content.ReadAsStringAsync();
			}
		}
	}
}