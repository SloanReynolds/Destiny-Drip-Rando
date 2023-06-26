using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using BungieSharper.Client;
using BungieSharper.Entities;
using BungieSharper.Entities.Destiny;

namespace DapperBungled {
	public class OAuth {
		private static readonly string _TOKEN_FILE_PATH = Directory.GetCurrentDirectory() + "\\tokens.txt";
		private const string STATE_CHARS = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789";

		public static string AccessToken => _accessToken;
		private static string _accessToken = "";
		private static DateTime _expires = DateTime.Now.AddSeconds(-120);
		private static DateTime _refreshExpires = DateTime.Now.AddSeconds(-120);
		private static string _refreshToken = "";
		private static IDapperContext? _context;

		private static bool _hasLoaded = false;

		public static void Validate(BungieApiClient client) {
			if (_context == null)
				throw new NullReferenceException("Context cannot be null! Are you sure you've told the OAuth the context correctly?");
			string stateVar = STATE_CHARS.ToCharArray().GetRandom(32);
			
			var authCode = _context.GetOAuth_AuthorizationCode(client.OAuth.GetOAuthAuthorizationUrl(stateVar), stateVar);

			var response = client.OAuth.GetOAuthToken(authCode).GetAwaiter().GetResult();
			_UpdateFromResponse(response);
		}

		private static void _UpdateFromResponse(TokenResponse response) {
			_accessToken = response.AccessToken;
			_expires = DateTime.Now.AddSeconds((double)response.ExpiresIn);
			_refreshToken = response.RefreshToken;
			_refreshExpires = DateTime.Now.AddSeconds((double)response.RefreshExpiresIn);

			_SaveToFile();
		}

		internal static void RefreshIfNeeded(BungieApiClient client) {
			if (!_hasLoaded) {
				_LoadFromFile();
			}

			if (_accessToken == "" || _refreshToken == "" || DateTime.Now >= _refreshExpires) {
				Validate(client);
			} else if (DateTime.Now >= _expires) {
				var response = client.OAuth.RefreshOAuthToken(_refreshToken).GetAwaiter().GetResult();
				_UpdateFromResponse(response);
			}
		}

		private static void _LoadFromFile() {
			if (File.Exists(_TOKEN_FILE_PATH)) {
				var arr = File.ReadAllLines(_TOKEN_FILE_PATH);
				if (arr.Length == 4) {
					_accessToken = arr[0];
					_refreshToken = arr[1];
					_expires = DateTime.Parse(arr[2]);
					_refreshExpires = DateTime.Parse(arr[3]);
				}
			}
			_hasLoaded = true;
		}

		private static void _SaveToFile() {
			File.WriteAllLines(_TOKEN_FILE_PATH, new string[] { _accessToken, _refreshToken, _expires.ToString(), _refreshExpires.ToString() });
		}

		public static void SetContext(IDapperContext contextStruct) {
			_context = contextStruct;
		}
	}
}
