using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BungieSharper.Entities.Destiny.Definitions;
using BungieSharper.Entities.Destiny.Definitions.Sockets;
using BungieSharper.Entities.Destiny.Entities.Items;

namespace DapperBungled {
	public class CustomItemSocket {
		public int Index { get; }
		public CustomPlug? Plug { get; private set; }
		public DestinySocketCategoryDefinition Category { get; }
		public DestinyItemSocketEntryDefinition Definition { get; }
		public IEnumerable<uint> HashWhiteList { get; }

		public CustomItemSocket(Profile profile, DestinyItemSocketState socket, int currentSocketIndex, DestinySocketCategoryDefinition socketCategory, DestinyItemSocketEntryDefinition entryDefinition) {
			this.Index = currentSocketIndex;
			if (socket.PlugHash != null) {
				this.Plug = CustomPlug.ForceGet(profile, socket.PlugHash.Value);
			}
			this.Category = socketCategory;
			this.Definition = entryDefinition;
			this.HashWhiteList = _GetHashWhiteList();
		}

		public void UpdateCurrentPlug(CustomPlug newPlug) {
			this.Plug = newPlug;
		}

		private IEnumerable<uint> _GetHashWhiteList() {
			if (Definition == null) { return new uint[] { }; }

			List<uint> hashWhileList = new();

			if (Definition.SingleInitialItemHash != 0) {
				hashWhileList.Add(Definition.SingleInitialItemHash);
			}
			foreach (var plugItem in Definition.ReusablePlugItems) {
				hashWhileList.Add(plugItem.PlugItemHash);
			}
			if (Definition.ReusablePlugSetHash != null && Definition.ReusablePlugSetHash != 0) {
				foreach (var plug in DefinitionStore.PlugSets[Definition.ReusablePlugSetHash.Value].ReusablePlugItems) {
					hashWhileList.Add(plug.PlugItemHash);
				}
			}

			return hashWhileList;
		}

		public override string ToString() {
			return (Plug == null ? "NULL" : Plug.Name) + " | " + (Category == null ? "NULL" : Category.DisplayProperties.Name);
		}

	}
}