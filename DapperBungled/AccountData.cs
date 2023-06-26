using System.Runtime.CompilerServices;
using BungieSharper.Entities.Destiny;
using BungieSharper.Entities.Destiny.Entities.Characters;
using BungieSharper.Entities.Destiny.Responses;

namespace DapperBungled {
	public class AccountData {
		public Profile Profile { get; }
		public Dictionary<long, CustomCharacter> Characters = new Dictionary<long, CustomCharacter>();
		public Dictionary<uint, CustomPlugSet> ProfilePlugSets = new Dictionary<uint, CustomPlugSet>();


		public IEnumerable<CustomPlug> ProfilePlugs => ProfilePlugSets.SelectMany(plugSet => plugSet.Value.Plugs).Distinct();

		public IEnumerable<CustomPlug> AllAccountPlugs => ProfilePlugs.Union(Characters.SelectMany(chr => chr.Value.Plugs)).Distinct();

		public void DebugPlugSets() {
			Console.WriteLine("PROFILE PLUGSETS");
			Console.WriteLine("----------------");
			foreach (var plugSet in ProfilePlugSets) {
				Console.WriteLine($"SET: {plugSet.Key}");
				foreach (var plug in plugSet.Value.Plugs) {
					Console.WriteLine("    " + plug.ToStringLong());
				}
			}
			Console.WriteLine("CHARACTER PLUGSETS");
			Console.WriteLine("----------------");
			foreach (var character in Characters.Values) {
				Console.WriteLine(character.Component.ClassType);
				foreach (var plugSet in ProfilePlugSets) {
					Console.WriteLine($"SET: {plugSet.Key}");
					foreach (var plug in plugSet.Value.Plugs) {
						Console.WriteLine("    " + plug.ToStringLong());
					}
				}
			}
		}

		public AccountData(Profile profile) {
			Profile = profile;
		}

		public void LoadProfile_ForInventory() {
			DestinyComponentType[] comps = new DestinyComponentType[] {
				DestinyComponentType.Characters,
				DestinyComponentType.ItemSockets,
				DestinyComponentType.CharacterInventories,
				DestinyComponentType.ProfileInventories,
			};
			var profileResponse = ApiCallsEZ.GetProfile(Profile, comps).GetAwaiter().GetResult();
			_LoadProfile(profileResponse);
		}

		public void LoadProfile_ForPlugSets() {
			DestinyComponentType[] comps = new DestinyComponentType[] {
				DestinyComponentType.Characters,
				DestinyComponentType.ItemSockets,
				DestinyComponentType.CharacterEquipment,
				DestinyComponentType.CharacterActivities
			};
			var profileResponse = ApiCallsEZ.GetProfile(Profile, comps).GetAwaiter().GetResult();
			_LoadProfile(profileResponse);
		}

		private void _LoadProfile(DestinyProfileResponse response) {
			//PlugSets
			if (response.ProfilePlugSets != null) {
				foreach (var kvpPlugSet in response.ProfilePlugSets.Data.Plugs) {
					ProfilePlugSets.Add(kvpPlugSet.Key, new CustomPlugSet(Profile, kvpPlugSet.Key, kvpPlugSet.Value));
				}
			}

			foreach (var kvpCharacter in response.Characters.Data) {
				var newChar = new CustomCharacter(kvpCharacter.Value);

				//PlugSets
				if (response.CharacterPlugSets != null) {
					foreach (var plugSet in response.CharacterPlugSets.Data[kvpCharacter.Key].Plugs) {
						newChar.AddPlugSet(new CustomPlugSet(Profile, plugSet.Key, plugSet.Value));
					}
				}

				//Equipment
				if (response.CharacterEquipment != null) {
					_UpdateCharacterEquipment(newChar, response);
				}

				//Inventory
				if (response.CharacterInventories != null) {
					_UpdateCharacterInventories(newChar, response);
				}

				//CharacterActivities
				if (response.CharacterActivities != null) {
					_UpdateCharacterActivities(newChar, response);
				}

				Characters.Add(kvpCharacter.Key, newChar);
			}
		}

		private void _UpdateCharacterInventories(CustomCharacter newChar, DestinyProfileResponse response) {
			foreach (var item in response.CharacterInventories.Data[newChar.ID].Items) {
				var newItem = new CustomItem(item);
				newChar.AddInventory(newItem);

				//Sockets? BY INSTANCE ID OF EQ!
				if (newItem.InstanceID != null && response.ItemComponents.Sockets.Data.ContainsKey(newItem.InstanceID.Value)) {
					foreach (var socket in response.ItemComponents.Sockets.Data[newItem.InstanceID.Value].Sockets) {
						newItem.AddSocketState(Profile, socket);
					}
				}
			}
		}

		public delegate void CharacterEquipmentUpdatedEventHandler(CustomCharacter chr);
		public event CharacterEquipmentUpdatedEventHandler OnCharacterEquipmentUpdated;

		private void _UpdateCharacterEquipment(CustomCharacter newChar, DestinyProfileResponse response) {
			newChar.ResetEquipment();
			foreach (var equipmentItem in response.CharacterEquipment.Data[newChar.ID].Items) {
				var newEq = new CustomItem(equipmentItem);
				newChar.AddEquipment(newEq);

				//Sockets? BY INSTANCE ID OF EQ!
				if (newEq.InstanceID != null && response.ItemComponents.Sockets.Data.ContainsKey(newEq.InstanceID.Value)) {
					foreach (var socket in response.ItemComponents.Sockets.Data[newEq.InstanceID.Value].Sockets) {
						newEq.AddSocketState(Profile, socket);
					}
				}
			}

			OnCharacterEquipmentUpdated?.Invoke(newChar);
		}

		public void UpdateData() {
			var profileResponse = ApiCallsEZ.GetProfile(Profile, DestinyComponentType.Characters, DestinyComponentType.CharacterActivities, DestinyComponentType.CharacterEquipment, DestinyComponentType.ItemSockets).GetAwaiter().GetResult();
			if (!ApiCallsEZ.WasLikelyCachedResponse(Profile))
				_UpdateData(profileResponse);
		}

		private void _UpdateData(DestinyProfileResponse response) {
			foreach (var character in response.Characters.Data) {
				Characters[character.Key].UpdateComponent(character.Value);
				_UpdateCharacterActivities(Characters[character.Key], response);
				_UpdateCharacterEquipment(Characters[character.Key], response);
			}
		}

		private void _UpdateCharacterActivities(CustomCharacter character, DestinyProfileResponse response) {
			//CharacterActivities
			character.AddActivities(response.CharacterActivities.Data[character.ID]);
		}
	}
}
