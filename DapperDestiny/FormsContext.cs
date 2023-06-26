using System.Diagnostics;
using System.Security.Policy;
using DapperBungled;

namespace DapperDestiny {
	internal class FormsContext : IDapperContext {
		HttpClient _client;

		public string GetOAuth_AuthorizationCode(string authURL, string state) {
			Process.Start(new ProcessStartInfo { FileName = authURL, UseShellExecute = true });
			MessageBox.Show($"You need to log into Bungie. ({authURL}) Click OK when finished.", "Log in to Bungie", MessageBoxButtons.OK);
			if (_client == null)
				_client = new HttpClient();

			var retVal = "";
			bool retry = true;
			while (retry) {
				retry = false;
				retVal = _client.GetStringAsync($"https://www.lanimals.com/ddr/auth.asp?poll=true&state={state}").GetAwaiter().GetResult();
				if (retVal.Contains("ANTEATER")) {
					var result = MessageBox.Show($"The auth code wasn't found; did you click OK too soon?", "ANTEATER", MessageBoxButtons.RetryCancel);
					if (result == DialogResult.Retry) {
						retry = true;
					} else {
						//idk i guess we'll crash? lol
						throw new NotImplementedException("User canceled the OAuth process, unsure how to proceed.");
					}
				}
			}
			return retVal;
		}
	}
}