using System.Diagnostics;
using DapperBungled;
using DapperDestiny.Properties;

namespace DapperDestiny {
	public partial class ItemImageBox : UserControl {
		private ItemImageObject _item = null;

		public ItemImageBox() {
			InitializeComponent();
			icon.Controls.Add(badge);
		}

		private void ItemImageBox_Load(object sender, EventArgs e) {
			//badge.Size = icon.Size;
		}

		public void Update(CustomPlug item) {
			if (item != null) {
				Update(new ItemImageObject(item));
			} else {
				UpdateIcon("", "");
			}
		}

		public void Update(CustomItem item) {
			Update(new ItemImageObject(item));
		}

		public void Update(ItemImageObject item) {
			_item = item;
			UpdateIcon(item.IconUrl, item.WatermarkUrl);

			if (badge.Visible) {
				Main.SetToolTip(badge, _item.ToolTip);
			} else if (icon.Visible) {
				Main.SetToolTip(icon, _item.ToolTip);
			}
		}

		public void UpdateIcon(string urlIcon, string urlBadge) {
			badge.BackgroundImage = Resources.WAIT;

			if (string.IsNullOrWhiteSpace(urlIcon)) {
				_Hide(icon);
			} else {
				_Show(icon);
				ContentCache.Retrieve(urlIcon, _UpdateIcon);
			}

			if (string.IsNullOrWhiteSpace(urlBadge)) {
				_Hide(badge);
			} else {
				_Show(badge);
				ContentCache.Retrieve(urlBadge, _UpdateBadge);
			}
		}

		private void _UpdateIcon(string url) {
			_UpdatePictureBox(this.icon, url, Resources.UntitledGear_Icon);
		}

		private void _UpdateBadge(string url) {
			_UpdatePictureBox(this.badge, url, Resources.Blank);
		}

		private void _UpdatePictureBox(PictureBox box, string url, Bitmap defaultImage) {
			if (url == "") {
				box.BackgroundImage = defaultImage;
			} else {
				box.BackgroundImage = Image.FromFile(url);
			}
		}

		private void _Show(PictureBox elem) {
			if (InvokeRequired) { Invoke(_Show, elem); return; }
			elem.Show();
		}

		private void _Hide(PictureBox elem) {
			if (InvokeRequired) { Invoke(_Hide, elem); return; }
			elem.Hide();
		}
	}
}
