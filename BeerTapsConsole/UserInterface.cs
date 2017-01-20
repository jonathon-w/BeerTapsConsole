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

		// Offices(x)/Beertaps
		public static Regex beerTapsAll = new Regex(@"\b(Offices\()\b\d+\)/\b(Beertaps)\b$");

		// Offices(x)/Beertaps(x)
		public static Regex beerTapsIndividual = new Regex(@"\b(Offices\()\b\d+\)/\b(Beertaps\()\b\d{1,2}\)");

		// Offices(x)/Beertaps(x)/ReplaceKeg
		public static Regex replaceKegAll = new Regex(@"\b(Offices\()\b\d+\)/\bBeertaps\b\(\b\d+\)/\bReplaceKeg\b");

		// // Offices(x)/Beertaps(x)/ReplaceKeg(x)
		public static Regex replaceKegIndividual = new Regex(@"\b(Offices\()\b\d+\)/\bBeertaps\b\(\b\d+\)/\bReplaceKeg\b\(\b\d+\)");

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

					Console.Write("=========================================\n" +
								  "|Enter Id, or <Esc> to exit : ");
					input = Console.ReadKey();
					Console.WriteLine("\n=========================================\n");

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
				else if (beerTapsAll.IsMatch(targetUri))
				{
					Task<string> targetResponse = HttpProtocols.GetResponseAsync(targetUri);
					IList<BeerTap> parsedResponse = ParseResponse<BeerTap>.ParseForAll(targetResponse.Result);
					Print.PrintAllBeerTaps(parsedResponse, officeLocation);

					Console.Write("=========================================\n" +
					              "|Enter Id, <Backspace> to go back, or <Esc> to exit : ");
					input = Console.ReadKey();
					Console.WriteLine("\n=========================================\n");

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
				else if (beerTapsIndividual.IsMatch(targetUri))
				{
					Task<string> targetResponse = HttpProtocols.GetResponseAsync(targetUri);
					BeerTap parsedResponse = ParseResponse<BeerTap>.ParseForIndividual(targetResponse.Result); 
					Print.PrintIndividualBeerTap(parsedResponse, officeLocation);

					Console.Write("=========================================\n" +
								  "|Enter Id, <Backspace> to go back, or <Esc> to exit : ");
					input = Console.ReadKey();
					Console.WriteLine("\n=========================================\n");

					if (input.Key == ConsoleKey.Backspace)
					{
						targetUri = UriStack.Pop();
					}
				}
				else
				{
					input = Console.ReadKey();
				}

				//				input = Console.ReadKey();

			} while (input.Key != ConsoleKey.Escape);
			

//			Console.Write("\n=========================================\n" +
//			              "|Enter Id : ");
//			input = Convert.ToInt32(Console.ReadLine());
//			Console.WriteLine("=========================================\n");
//			
//			string nextTargetUri = default(string);
//			foreach (Office office in parsedResponse)
//			{
//				if (office.Id == input)
//				{
//					nextTargetUri = UriBase + office.Links.IqBeerTaps.Href;
//					break;
//				}
//			}
//			if (nextTargetUri != default(string))
//			{
//				Task<string> nextTargetResponse = HttpProtocols.GetResponseAsync(nextTargetUri);
//				Task.WaitAll();
//				IList<BeerTap> nextParsedResponse = ParseResponse<BeerTap>.ParseForAll(nextTargetResponse.Result);
//
//				Print.PrintAllBeerTaps(nextParsedResponse, parsedResponse[input - 1].Location);
//			}
//			else
//			{
//				Console.WriteLine("{0} is not valid", input);
//			}
//			
//			Console.ReadLine();
		}
	}
}
