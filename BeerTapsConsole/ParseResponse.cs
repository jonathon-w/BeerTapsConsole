using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BeerTapsConsole
{
	public static class ParseResponse<T>
	{
		public static IList<T> ParseForAll(string response)
		{
			JObject responseObject = JObject.Parse(response);

			IList<JToken> results = responseObject["_embedded"]["self"].Children().ToList();

			IList<T> objectsList = new List<T>();
			foreach (JToken result in results)
			{
				T singleObject = JsonConvert.DeserializeObject<T>(result.ToString());
				objectsList.Add(singleObject);
			}
			return objectsList;
		}

		public static T ParseForIndividual(string response)
		{
			JToken responseObject = JToken.Parse(response);

			T returnObject = JsonConvert.DeserializeObject<T>(responseObject.ToString());
			
			return returnObject;
		}
	}
}