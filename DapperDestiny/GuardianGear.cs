using System.Diagnostics;
using DapperBungled;

namespace DapperDestiny {
	public partial class GuardianGear : UserControl {
		public CustomCharacter CurrentCharacter { get; private set; }
		private List<GearPiece> _all;

		public GuardianGear() {
			InitializeComponent();
			
			_all = new() {
				gearKinetic.AsSlot(ItemSlot.Kinetic),
				gearEnergy.AsSlot(ItemSlot.Energy),
				gearPower.AsSlot(ItemSlot.Power),
				gearHead.AsSlot(ItemSlot.Head),
				gearArms.AsSlot(ItemSlot.Arms),
				gearChest.AsSlot(ItemSlot.Chest),
				gearLegs.AsSlot(ItemSlot.Legs),
				gearClass.AsSlot(ItemSlot.Class)
			};
		}

		public void SetCharacter(CustomCharacter character) {
			CurrentCharacter = character;
			UpdateEquipment();
		}

		public void UpdateEquipment() {
			foreach (var item in CurrentCharacter.Equipment.Values) {
				UpdatePiece(item);
			}
		}

		public void UpdatePiece(CustomItem item) {
			var gear = _all.FirstOrDefault(g => g.Slot == item.Slot);
			if (gear != null) {
				gear.UpdateItem(item);
			}
		}
	}
}
