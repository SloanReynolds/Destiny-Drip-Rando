using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BungieSharper.Entities.Destiny.Definitions;
using BungieSharper.Entities.Destiny.Definitions.Sockets;
using BungieSharper.Entities.Destiny.Entities.Items;

namespace DapperBungled {
	public enum ItemSlot {
		Kinetic = 1498876634,
		Energy = -1829672231,
		Power = 953998645,
		Head = -846692857,
		Arms = -743048708,
		Chest = 14239492,
		Legs = 20886954,
		Class = 1585787867,

		UNKNOWN = 0
	}

	public class CustomItem {
		public long? InstanceID { get; }
		public uint ItemHash { get; }
		public DestinyItemComponent ItemComponent { get; }
		public DestinyInventoryItemDefinition Definition { get; }

		public string Name => Definition.DisplayProperties.Name;

		public IList<CustomItemSocket> SocketPlugs => _socketPlugs;

		private List<CustomItemSocket> _socketPlugs = new();

		private int _ornamentSocketIndex = -1;
		private int _shaderSocketIndex = -1;

		public CustomItemSocket ShaderSocket => _shaderSocketIndex > -1 ? _socketPlugs[_shaderSocketIndex] : null;
		public CustomItemSocket OrnamentSocket => _ornamentSocketIndex > -1 ? _socketPlugs[_ornamentSocketIndex] : null;
		public bool OrnamentIsDefault {
			get {
				if (OrnamentSocketExists
					&& OrnamentSocket.Plug != null
					&& OrnamentSocket.Plug.Hash == OrnamentSocket.Definition.SingleInitialItemHash
					) {
					return true;
				}
				return false;
			}
		}

		public bool OrnamentSocketExists => OrnamentSocket != null;

		public ItemSlot Slot => (ItemSlot)Definition.EquippingBlock.EquipmentSlotTypeHash;


		public CustomItem(DestinyItemComponent eq) {
			this.InstanceID = eq.ItemInstanceId;
			this.ItemHash = eq.ItemHash;
			this.ItemComponent = eq;
			this.Definition = DefinitionStore.Items[this.ItemHash];
		}

		internal void AddSocketState(Profile profile, DestinyItemSocketState socket) {
			var currentSocketIndex = _socketPlugs.Count;
			DestinySocketCategoryDefinition socketCategory = null;
			DestinyItemSocketEntryDefinition blockDefinition = null;
			if (Definition.Sockets != null) {
				var temp = Definition.Sockets.SocketCategories.FirstOrDefault(cat => cat.SocketIndexes.Contains(currentSocketIndex));
				if (temp != null) {
					socketCategory = DefinitionStore.SocketCategories[temp.SocketCategoryHash];
				}
				blockDefinition = Definition.Sockets.SocketEntries.ToArray()[currentSocketIndex];
			}
			var socketPlug = new CustomItemSocket(profile, socket, currentSocketIndex, socketCategory, blockDefinition);
			_socketPlugs.Add(socketPlug);

			//Store friendly socket types
			if (socketPlug.Definition.SingleInitialItemHash > 0) {
				string socketInitialItemName = DefinitionStore.Items[socketPlug.Definition.SingleInitialItemHash].DisplayProperties.Name;
				if (socketInitialItemName.Contains("Ornament")) {
					_ornamentSocketIndex = currentSocketIndex;
				} else if (socketInitialItemName.Contains("Shader")) {
					_shaderSocketIndex = currentSocketIndex;
				}
			}
		}
	}
}
