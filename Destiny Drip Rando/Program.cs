using System.Data.Entity.Core.Metadata.Edm;
using BungieSharper.Client;
using BungieSharper.Entities.Destiny.HistoricalStats.Definitions;
using BungieSharper.Entities.Exceptions;
using DapperBungled;
using Destiny_Drip_Rando;

internal class Program {
	private const bool CATCH_EXCEPTIONS = true;
	private const uint ORBIT_ACTIVITY_HASH = 82913930;
	private const DestinyActivityModeType SOCIAL_ACTIVITY_TYPE = DestinyActivityModeType.Social;
	private const int TIME_BETWEEN_CHECKS = 10 * 1000;

	private static long _previousCharacterId = 0;
	private static uint _previousActivityHash = 0;
	private enum _SkipType {
		None,
		SameActivity,
		InvalidActivity
	}
	private static _SkipType _skipType = _SkipType.None;

	private static async Task _RandoMeBaby(Profile profile, AccountData data) {
		data.UpdateData();
		var character = data.Characters.Values.OrderByDescending(chr => chr.LastPlayed).First();

		if (_previousCharacterId != character.ID) {
			_previousActivityHash = 0;
			_previousCharacterId = character.ID;
		}

		//Valid Activity?
		if (character.ActivityHash == _previousActivityHash) {
			//We are in the same activity as last time

			if (_skipType != _SkipType.SameActivity) { //Only notify once in a row, plz!
				Console.WriteLine($"{DateTime.Now} - Skipping: we haven't changed activities! ([{character.ActivityType}]{character.ActivityHash} / {_previousActivityHash})");
				_skipType = _SkipType.SameActivity;
			}

			return;
		}

		_previousActivityHash = character.ActivityHash;

		if (character.ActivityType != SOCIAL_ACTIVITY_TYPE && character.ActivityHash != ORBIT_ACTIVITY_HASH) {
			//We aren't in a social space

			if (_skipType != _SkipType.InvalidActivity) { //Only notify once in a row, plz!
				Console.WriteLine($"{DateTime.Now} - Skipping: we aren't somewhere we can change equipment! ([{character.ActivityType}]{character.ActivityHash} / {_previousActivityHash})");
				_skipType = _SkipType.InvalidActivity;
			}
			return;
		}

		//Made it past the skip conditions. Hooray!
		Console.WriteLine($"{DateTime.Now} - Randomizing... ([{character.ActivityType}]{character.ActivityHash} / {_previousActivityHash})");
		_skipType = _SkipType.None;

		foreach (var item in character.Equipment.Values) {
			//Console.WriteLine(item.Name);
			foreach (var socket in item.SocketPlugs.Where(csp => csp.Category != null && csp.Category.DisplayProperties.Name.Contains("COSMETIC"))) {
				if (item.InstanceID == null) { continue; }
				//Console.WriteLine("  " + plug.Index + " " + plug.ToString());
				var profileFilteredWhiteList
					= data.ProfilePlugs.Where(pp => pp.IsReady && socket.HashWhiteList.Contains(pp.Hash));

				if (!profileFilteredWhiteList.Select(cp => cp.Hash).Contains(socket.Definition.SingleInitialItemHash)) {
					profileFilteredWhiteList = profileFilteredWhiteList.Prepend(CustomPlug.ForceGet(profile, socket.Definition.SingleInitialItemHash));
				}

				//foreach (var pp in profileFilteredWhiteList) {
				//	Console.WriteLine("    " + pp.ToStringLong());
				//}

				var randomPlug = profileFilteredWhiteList.GetRandom();
				if (randomPlug.Hash != socket.Plug.Hash) {
					try {
						await ApiCallsEZ.InsertPlugFree(character, item, socket, randomPlug);
					} catch (BungieApiNoRetryException ex) {
						if (ex.ApiResponseMsg.ErrorCode != PlatformErrorCodes.DestinyCharacterNotInTower) {
							throw;
						}
						Console.WriteLine($"{DateTime.Now} - Stopped rando; you're busy, silly!");
						//Thread.Sleep(60 * 1000);
						return;
					}
				}
			}
		}
		Console.WriteLine(DateTime.Now + " - Success!");
		//_nextRun = DateTime.Now.AddMinutes(5);
	}

	private static async Task Main(string[] args) {
		OAuth.SetContext(new ConsoleContext());
		await ApiCallsEZ.InitializeSQLite();

		Profile profile = new Profile(4611686018467365389);

		AccountData data = new AccountData(profile);
		data.LoadProfile_ForPlugSets();

		while (true) {
			try {
				await _RandoMeBaby(profile, data);
			} catch (Exception ex) {
				Console.WriteLine(ex.ToString());
			}

			Thread.Sleep(TIME_BETWEEN_CHECKS);
		}
	}
}
//}