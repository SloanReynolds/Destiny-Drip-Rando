using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperBungled;

namespace DapperDestiny {
	public class ItemImageObject {
		private bool _isPlug = false;
		private CustomItem _item;
		private CustomPlug _plug;

		public ItemImageObject(CustomItem item) {
			this._item = item;
		}

		public ItemImageObject(CustomPlug plug) {
			this._plug = plug;
			_isPlug = true;
		}

		public string IconUrl => _isPlug ? _plug.ItemDefinition.DisplayProperties.Icon : _item.Definition.DisplayProperties.Icon;
		public string WatermarkUrl => _isPlug ? _plug.ItemDefinition.IconWatermark : _item.Definition.IconWatermark;

		public string ToolTip => _isPlug ? _plug.Name : _item.Name;
	}
}
