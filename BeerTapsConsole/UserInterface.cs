using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BeerTapsConsole
{
	class UserInterface
	{
		public const string UriBase = "http://localhost:61284/";
		
		static void Main()
		{
			ConsoleKeyInfo input;
			
			// Set the initial target to the homepage, /Offices
			string targetUri = UriBase + "Offices";

			string officeLocation = default(string);

			// A stack to hold the previous URIs so that I can go back a page with <Backspace>
			Stack<string> UriStack = new Stack<string>();
			UriStack.Push(targetUri); // Not really necessary

			do
			{
				// /Offices
				if (targetUri == UriBase + "Offices")
				{
					Task<string> targetResponse = HttpProtocols.GetResponseAsync(targetUri);
					IList<Office> parsedResponse = ParseResponse<Office>.ParseForAll(targetResponse.Result);
					Print.PrintAllOffices(parsedResponse);

					Console.WriteLine("=========================================\n" +
									  "|<1-9> : View individual office\n" +
									  "|<Esc> : Exit\n" +
									  "=========================================\n");
					input = Console.ReadKey();

					foreach (Office office in parsedResponse)
					{
						if (char.IsDigit(input.KeyChar) && office.Id == int.Parse(input.KeyChar.ToString()))
						{
							UriStack.Push(targetUri);
							targetUri = UriBase + office.Links.IqBeerTaps.Href;

							officeLocation = office.Location;
							break;
						}
					}
				}
				// /Offices(x)/Beertaps
				else if (UriRegex.BeerTapsAll.IsMatch(targetUri))
				{
					Task<string> targetResponse = HttpProtocols.GetResponseAsync(targetUri);
					IList<BeerTap> parsedResponse = ParseResponse<BeerTap>.ParseForAll(targetResponse.Result);
					Print.PrintAllBeerTaps(parsedResponse, officeLocation);

					Console.WriteLine("=========================================\n" +
									  "|<1-9> : View individual tap\n" +
									  "|<Backspace> : Back\n" +
									  "|<Esc> : Exit\n" +
									  "=========================================\n");
					input = Console.ReadKey();

					foreach (BeerTap beerTap in parsedResponse)
					{
						if (char.IsDigit(input.KeyChar) && beerTap.Id == int.Parse(input.KeyChar.ToString()))
						{
							UriStack.Push(targetUri);
							targetUri = UriBase + beerTap.Links.Self.Href;

							break;
						}
					}

					if (input.Key == ConsoleKey.Backspace)
					{
						targetUri = UriStack.Pop();
					}
				}
				// /Offices(x)/Beertaps(x)
				else if (UriRegex.BeerTapsIndividual.IsMatch(targetUri))
				{
					Task<string> targetResponse = HttpProtocols.GetResponseAsync(targetUri);
					BeerTap parsedResponse = ParseResponse<BeerTap>.ParseForIndividual(targetResponse.Result); 
					Print.PrintIndividualBeerTap(parsedResponse, officeLocation);

					Console.WriteLine("=========================================\n" +
									  "|<1> : Pour a pint\n" +
									  "|<2> : View replacement kegs\n" +
									  "|<Backspace> : Back\n" +
									  "|<Esc> : Exit\n" +
									  "=========================================\n");
					input = Console.ReadKey();

					if (input.Key == ConsoleKey.Backspace)
					{
						targetUri = UriStack.Pop();
					}
					else if (input.Key == ConsoleKey.D1)
					{
						dynamic jsonObject = new JObject();
						jsonObject.OfficeId = parsedResponse.OfficeId;
						jsonObject.Id = parsedResponse.Id;

						var postContent = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");

						Task postTargetResponse = HttpProtocols.PostRequestAsync(targetUri, postContent);
//						BeerTap newParsedResponse = ParseResponse<BeerTap>.ParseForIndividual(postTargetResponse.Result);
//						Print.PrintIndividualBeerTap(newParsedResponse, officeLocation);
					}
				}
				else
				{
					input = Console.ReadKey();
				}

			} while (input.Key != ConsoleKey.Escape);
		}

		private static class UriRegex
		{
			// Offices(x)/Beertaps
			public static readonly Regex BeerTapsAll = new Regex(@"\b(Offices\()\b\d+\)/\b(Beertaps)\b$");

			// Offices(x)/Beertaps(x)
			public static readonly Regex BeerTapsIndividual = new Regex(@"\b(Offices\()\b\d+\)/\b(Beertaps\()\b\d{1,2}\)");

			// Offices(x)/Beertaps(x)/ReplaceKeg
			public static readonly Regex ReplaceKegAll = new Regex(@"\b(Offices\()\b\d+\)/\bBeertaps\b\(\b\d+\)/\bReplaceKeg\b");

			// // Offices(x)/Beertaps(x)/ReplaceKeg(x)
			public static readonly Regex ReplaceKegIndividual = new Regex(@"\b(Offices\()\b\d+\)/\bBeertaps\b\(\b\d+\)/\bReplaceKeg\b\(\b\d+\)");
		}
	}
}
