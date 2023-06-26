using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DapperBungled;

namespace DapperDestiny {
	public partial class GearPiece : UserControl {
		public GearPiece() {
			InitializeComponent();
		}

		public ItemSlot Slot { get; private set; }

		private void GearPiece_Load(object sender, EventArgs e) {

		}

		public void UpdateItem(CustomItem item) {
			if (InvokeRequired) {
				Invoke(UpdateItem, item);
				return;
			}

			gearbox.Text = item.Name;

			itemImageBig.Update(item);
			itemImageSmallTop.Update(item.ShaderSocket?.Plug);
			itemImageSmallBottom.Update(item.OrnamentSocket?.Plug);
			return;

			if (!item.OrnamentSocketExists) {
				itemImageBig.Update(item);
				itemImageSmallTop.Update(item.ShaderSocket?.Plug);
				itemImageSmallBottom.Update((CustomPlug)null);
				return;
			}

			if (item.OrnamentIsDefault) {
				itemImageBig.Update(item);
				itemImageSmallTop.Update(item.ShaderSocket?.Plug);
				itemImageSmallBottom.Update(item);
				return;
			}

			itemImageBig.Update(item.OrnamentSocket?.Plug);
			itemImageSmallTop.Update(item.ShaderSocket?.Plug);
			itemImageSmallBottom.Update(item);
		}

		internal GearPiece AsSlot(ItemSlot slot) {
			Slot = slot;
			return this;
		}
	}
}
