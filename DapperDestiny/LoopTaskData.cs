using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using DapperBungled;

namespace DapperDestiny {
	internal class LoopTaskData {
		public CustomPlug PreviousPlug { get; private set; }
		public CustomItem CurrentItem { get; private set; }
		public CustomPlug CurrentPlug { get; private set; }
		public CustomItemSocket CurrentSocket { get; private set; }

		internal string GetLastTaskString() {
			return $"{CurrentItem?.Name}[{CurrentSocket?.Index}] ({PreviousPlug?.Name}) <= {CurrentPlug?.Name}";
		}

		internal void SetCurrentTaskData(CustomItem item, CustomPlug randomPlug, CustomItemSocket socket) {
			PreviousPlug = item.SocketPlugs[socket.Index].Plug;

			CurrentItem = item;
			CurrentPlug = randomPlug;
			CurrentSocket = socket;

			Debug.WriteLine(GetLastTaskString());
		}
	}
}
