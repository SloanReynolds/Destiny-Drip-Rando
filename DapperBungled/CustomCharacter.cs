using BungieSharper.Entities.Destiny.Entities.Characters;
using BungieSharper.Entities.Destiny.HistoricalStats.Definitions;
using BungieSharper.Entities.Destiny.Responses;

namespace DapperBungled {
	public class CustomCharacter {
		public DestinyCharacterComponent Component { get; private set; }
		public Profile Profile { get; }
		public DestinyCharacterActivitiesComponent Activities { get; private set; }


		public Dictionary<uint, CustomPlugSet> PlugSets = new();
		public Dictionary<uint, CustomItem> Inventory = new();
		public Dictionary<uint, CustomItem> Equipment = new();

		public long ID => Component.CharacterId;
		public IEnumerable<CustomPlug> Plugs => PlugSets.SelectMany(plugSet => plugSet.Value.Plugs);

		public DateTime LastPlayed => Component.DateLastPlayed;

		public uint ActivityHash => Activities.CurrentActivityHash;
		public DestinyActivityModeType? ActivityType => (DestinyActivityModeType?)Activities.CurrentActivityModeType;


		//public DestinyActivityModeType LastActivityType;

		public CustomCharacter(DestinyCharacterComponent chr) {
			UpdateComponent(chr);
			Profile = new Profile(chr.MembershipId, chr.MembershipType);
		}

		internal void AddPlugSet(CustomPlugSet plugSet) {
			PlugSets.Add(plugSet.Hash, plugSet);
		}

		internal void AddInventory(CustomItem customItem) {
			Inventory.Add(customItem.ItemHash, customItem);
		}

		internal void AddEquipment(CustomItem customItem) {
			Equipment.Add(customItem.ItemHash, customItem);
		}

		internal void AddActivities(DestinyCharacterActivitiesComponent destinyCharacterActivitiesComponent) {
			this.Activities = destinyCharacterActivitiesComponent;
		}

		internal void UpdateComponent(DestinyCharacterComponent chr) {
			Component = chr;
		}

		internal void ResetEquipment() {
			Equipment.Clear();
		}
	}
}
