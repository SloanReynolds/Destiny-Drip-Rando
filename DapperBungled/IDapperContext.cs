using BungieSharper.Client;
using BungieSharper.Entities;

namespace DapperBungled {
	public interface IDapperContext {
		/// <summary>
		/// User must log in with Bungie; use the authURL to get the user to do so and return the Authorization Code from https://lanimals.com/ddr/auth.asp?state={stateGUID}
		/// </summary>
		/// <param name="authURL">Bungie's OAuth URL, including state variable</param>
		/// <param name="state">The state variable that uniquely identifies a single request</param>
		/// <returns></returns>
		public string GetOAuth_AuthorizationCode(string authURL, string state);
	}
}
