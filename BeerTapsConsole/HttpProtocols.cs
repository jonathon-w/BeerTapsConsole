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

		public static async Task PostRequestAsync(string target, StringContent formContent)
		{
			HttpClient client = new HttpClient();
			await client.PostAsync(target, formContent);

//			using (HttpClient client = new HttpClient())
//			using (HttpResponseMessage response = await client.PostAsync(target, formContent))
//			using (HttpContent content = response.Content)
//			{
//				return await content.ReadAsStringAsync();
//			}
		}
	}
}