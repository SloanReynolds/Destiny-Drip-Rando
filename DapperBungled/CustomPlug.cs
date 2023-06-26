using System.Net.Sockets;
using BungieSharper.Entities.Destiny.Definitions;
using BungieSharper.Entities.Destiny.Sockets;

namespace DapperBungled {
	public record CustomPlug {
		public DestinyItemPlug PlugComponent { get; private set; }
		public DestinyInventoryItemDefinition ItemDefinition { get; }

		public uint Hash => ItemDefinition.Hash;
		public string Name => ItemDefinition.DisplayProperties.Name;

		public bool IsReady => PlugComponent != null && PlugComponent.Enabled && PlugComponent.CanInsert;

		public string NameWithCategory { get; internal set; }
		

		public CustomPlug(Profile profile, uint hash) {
			this.ItemDefinition = DefinitionStore.Items[hash];

			_all.Add((profile, hash), this);
		}

		public CustomPlug(Profile profile, DestinyItemPlug plugComponent) : this(profile, plugComponent.PlugItemHash) {
			this.PlugComponent = plugComponent;
		}

		public string ToStringLong() {
			if (PlugComponent == null) {
				return $"{this.Hash} {this.Name} - NULL PLUG";
			}
			return $"{this.Hash} {this.Name} - {this.PlugComponent.CanInsert} {this.PlugComponent.Enabled} {(this.PlugComponent.EnableFailIndexes != null ? string.Join(",", this.PlugComponent.EnableFailIndexes) : "")} {(this.PlugComponent.InsertFailIndexes != null ? string.Join(",", this.PlugComponent.InsertFailIndexes) : "")}";
		}






		#region STATIC
		private static Dictionary<(Profile profile, uint plugHash), CustomPlug> _all = new();

		public static CustomPlug ForceGet(Profile profile, uint hash) {
			if (TryGet((profile, hash), out var plug)) {
				return plug;
			} else {
				return new CustomPlug(profile, hash);
			}
		}

		internal static CustomPlug ForceGetWithComponent(Profile profile, DestinyItemPlug plugComp) {
			if (TryGet((profile, plugComp.PlugItemHash), out var plug)) {
				if (plug.PlugComponent == null) {
					plug.UpdateComponent(plugComp);
				}
				return plug;
			}
			return new CustomPlug(profile, plugComp);
		}

		public static bool TryGet((Profile, uint) key, out CustomPlug plug) {
			if (_all.ContainsKey(key)) {
				plug = _all[key];
				return true;
			}

			plug = null;
			return false;
		}

		internal void UpdateComponent(DestinyItemPlug plugComp) {
			this.PlugComponent = plugComp;
		}
		#endregion
	}
}
