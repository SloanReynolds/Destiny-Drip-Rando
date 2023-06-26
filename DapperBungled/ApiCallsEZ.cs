using System.Data;
using System;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO.Compression;
using System.Threading;
using BungieSharper.Client;
using BungieSharper.Entities.Destiny;
using BungieSharper.Entities.Destiny.Config;
using BungieSharper.Entities.Destiny.Entities.Characters;
using BungieSharper.Entities.Destiny.Requests.Actions;
using BungieSharper.Entities.Destiny.Responses;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;

namespace DapperBungled {
	public class ApiCallsEZ {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
		//Disabled Warning: Must set at Run-Time
		private static string _currentContentName = "";
		private static DestinyManifest _manifest = null;
		private static SQLiteConnection _conn = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
		private static Dictionary<int, (int hash, bool collision)> _profileCallHashes = new();

		public static BungieApiClient _bCli = new BungieApiClient(new BungieClientConfig {
			ApiKey = "94c7dc6f174346a08b10a46518c95e59",
			UserAgent = "DestinyDripRando_Console/" + AppInfo.Version,
			OAuthClientId = AppInfo.ClientID,
			OAuthClientSecret = "HuIp-BgrMZ1FDOYIqFqnLu7J1oVmM0npPDLTn5dzFsU"
		});


		internal static bool WasLikelyCachedResponse(Profile profile) {
			//return false;
			return _profileCallHashes[profile.GetHashCode()].collision;
		}

		private static void _LogProfileCallHash(Profile profile, DestinyProfileResponse response) {
			//return;

			int responseHash = JsonSerializer.Serialize(response).GetHashCode();
			Debug.WriteLine("profile# = " + responseHash);
			if (!_profileCallHashes.ContainsKey(profile.GetHashCode())) {
				_profileCallHashes.Add(profile.GetHashCode(), (responseHash, false));
				return;
			}

			if (_profileCallHashes[profile.GetHashCode()].hash == responseHash) {
				_profileCallHashes[profile.GetHashCode()] = (responseHash, true);
				Debug.WriteLine("Response was Likely Cached");
				return;
			}

			_profileCallHashes[profile.GetHashCode()] = (responseHash, false);
		}

		public async static Task<DestinyProfileResponse> GetProfile(Profile profile, params DestinyComponentType[] componentTypes) {
			OAuth.RefreshIfNeeded(_bCli);
			var response = await _bCli.Api.Destiny2_GetProfile(
				profile.ID,
				profile.Type,
				componentTypes,
				OAuth.AccessToken
			);
			_LogProfileCallHash(profile, response);
			return response;
		}

		public static async Task InsertPlugFree(CustomCharacter character, CustomItem item, CustomItemSocket socket, CustomPlug plugToInsert) {
			if (item.InstanceID == null) { return; }

			//Debug.WriteLine($"{item.InstanceID} {item.ItemHash} <= {plugToInsert.Name} {plugToInsert.Hash} {plugToInsert.ItemDefinition.Hash}");
			await _InsertPlugFree(character.Component, item.InstanceID.Value, plugToInsert.Hash, socket.Index);
			socket.UpdateCurrentPlug(plugToInsert);
		}

		private static async Task<DestinyItemChangeResponse> _InsertPlugFree(DestinyCharacterComponent character, long itemInstanceID, uint plugHash, int socketIndex, DestinySocketArrayType socketType = DestinySocketArrayType.Default) {
			OAuth.RefreshIfNeeded(_bCli);
			var req = new DestinyInsertPlugsFreeActionRequest() {
				CharacterId = character.CharacterId,
				ItemId = itemInstanceID,
				MembershipType = character.MembershipType,
				Plug = new DestinyInsertPlugsRequestEntry() {
					PlugItemHash = plugHash,
					SocketIndex = socketIndex,
					SocketArrayType = socketType
				}
			};
			return await _bCli.Api.Destiny2_InsertSocketPlugFree(req, OAuth.AccessToken);
		}

		public async static Task<bool> CheckManifest() {
			var tempManifest = await _bCli.Api.Destiny2_GetDestinyManifest();

			if (_currentContentName != tempManifest.MobileWorldContentPaths["en"]) {
				_manifest = tempManifest;
				_currentContentName = _manifest.MobileWorldContentPaths["en"];

				return true;
			}

			return false;
		}

		public async static Task InitializeSQLite(int corruptDBAttempt = 1) {
			if (await CheckManifest() || _conn == null) {
				//Do the stuff
			} else {
				//current manifest is already loaded and good!
				return;
			}

			string downloadDir = Path.Join(Directory.GetCurrentDirectory(), "download");
			ContentCache.UpdateDownloadDir(downloadDir);
			string fileName = Path.GetFileName(_manifest.MobileWorldContentPaths["en"]);
			string filePath = Path.Join(downloadDir, fileName);

			//Make Download Folder
			if (!Directory.Exists(downloadDir)) {
				Directory.CreateDirectory(downloadDir);
			}

			//.content file download
			//New Manifest means new images to cache
			if (!File.Exists($"{filePath}")) {
				_ClearDirectoryContents(downloadDir);

				ContentCache.CreateDirectory(downloadDir);

				await ContentCache.AsyncDownloadFileTo(_manifest.MobileWorldContentPaths["en"], filePath);
			}

			//.content file unzip
			string zipPath = downloadDir + "\\extract\\";
			string zipFile = zipPath + fileName;
			if (!Directory.Exists(zipPath)) {
				Directory.CreateDirectory(zipPath);
			}

			if (!File.Exists($"{zipFile}")) {
				//Clear out the extract folder
				_ClearDirectoryContents(zipPath);

				ZipFile.ExtractToDirectory(filePath, zipPath);
			}

			//Now do SQL stuff finally
			_conn = new SQLiteConnection($"Data Source={zipFile};");
			_conn.Open();

			try {
				DefinitionStore.LoadTables(_conn);
			} catch (SQLiteException ex) {
					Debug.WriteLine(corruptDBAttempt);
				Debug.WriteLine(ex);
				if (ex.HResult == -2147481617 && corruptDBAttempt <= 3) {
					//Corrupt DB, we should delete all files and try to download the DB again;
					_conn.Close();

					_ClearDirectoryContents(downloadDir);
					_ClearDirectoryContents(zipPath);

					await InitializeSQLite(corruptDBAttempt+1);
				} else throw;
			}
		}

		private static void _ClearDirectoryContents(string dir) {
			//Clear out the download folder
			foreach (string file in Directory.GetFiles(dir)) {
				File.Delete(file);
			}
		}

		//internal static async Task<IDictionary<uint, CustomCollectible_Old>> GetCollectiblesForProfile(ProfileRecord profile)
		//	=> await GetCollectiblesForProfile(profile.ID, profile.Type);


		//internal static async Task<IDictionary<uint, CustomCollectible_Old>> GetCollectiblesForProfile(long profileID, BungieMembershipType membershipType = BungieMembershipType.TigerSteam) {
		//	var profile = await _bCli.Api.Destiny2_GetProfile(
		//		profileID,
		//		membershipType,
		//		new DestinyComponentType[] { DestinyComponentType.Collectibles, DestinyComponentType.ItemSockets }
		//	);

		//	var components = profile.ProfileCollectibles.Data.Collectibles;

		//	var newDict = new Dictionary<uint, CustomCollectible_Old>();
		//	foreach (var key in components.Keys) {
		//		newDict.Add(key, new CustomCollectible_Old(key, components[key], _defs.Collectibles[key]));
		//	}

		//	return newDict;
		//}
	}
}
