using System;
using System.Collections.Generic;

namespace BeerTapsConsole
{
	public static class Print
	{
		private static void PrintHeader()
		{
			Console.Clear();
			Console.WriteLine("=========================================\n" +
			                  "============= WHAT'S ON TAP =============\n" +
			                  "=========================================\n");
		}

		public static void PrintAllOffices(IList<Office> offices)
		{
			// Clear the console and print the header
			PrintHeader();

			Console.WriteLine("=========================================\n" +
							  "|Offices\n" +
							  "=========================================");

			foreach (Office office in offices)
			{
				Console.WriteLine(
					"=========================================\n" +
					"|Id : {0}\n" +
					"|Location : {1}\n" +
					"=========================================",
					office.Id,
					office.Location);
			}
		}

		public static void PrintAllBeerTaps(IList<BeerTap> beerTaps, string OfficeLocation)
		{
			// Clear the console and print the header
			PrintHeader();

			Console.WriteLine("=========================================\n" +
							  "|Beertaps at {0}\n" +
							  "=========================================",
							  OfficeLocation);

			foreach (BeerTap beerTap in beerTaps)
			{
				Console.WriteLine(
					"=========================================\n" +
					"|Id : {0}\n" +
					"|Beer : {1}\n" +
					"|Volume : {2}/{3}\n" +
					"=========================================",
					beerTap.Id,
					beerTap.BeerName,
					beerTap.CurrentVolume,
					beerTap.TotalVolume);
			}
		}

		public static void PrintIndividualBeerTap(BeerTap beerTap, string OfficeLocation)
		{
			// Clear the console and print the header
			PrintHeader();

			Console.WriteLine("=========================================\n" +
							  "|Beertap {0} at {1}\n" +
							  "=========================================",
							  beerTap.Id,
							  OfficeLocation);

			Console.WriteLine(
				"=========================================\n" +
				"|Id : {0}\n" +
				"|Beer : {1}\n" +
				"|Volume : {2}/{3}\n" +
				"=========================================",
				beerTap.Id,
				beerTap.BeerName,
				beerTap.CurrentVolume,
				beerTap.TotalVolume);
		}
	}
}