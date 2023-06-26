using BungieSharper.Client;
using BungieSharper.Entities;
using DapperBungled;

namespace Destiny_Drip_Rando {
	internal class ConsoleContext : IDapperContext {
		private HttpClient _client = new HttpClient();

		public string GetOAuth_AuthorizationCode(string authURL, string state) {
			Console.WriteLine($"Visit this URL: {authURL}");
			Console.WriteLine("Press Enter when finished logging in.");
			Console.ReadLine();

			string input = _client.GetStringAsync($"https://lanimals.com/ddr/auth.asp?poll=true&state={state}").GetAwaiter().GetResult();

			return input;
		}
	}
}
