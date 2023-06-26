using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BungieSharper.Entities.Destiny.Definitions.Sockets;
using BungieSharper.Entities.Destiny.Sockets;

namespace DapperBungled {
	public class CustomPlugSet {
		public uint Hash { get; }
		public IEnumerable<CustomPlug> Plugs => _plugs;
		private IEnumerable<CustomPlug> _plugs;

		public DestinyPlugSetDefinition Definition { get; }

		public CustomPlugSet(Profile profile, uint plugSetHash, IEnumerable<DestinyItemPlug> value) {
			this.Hash = plugSetHash;
			this._plugs = value.Select(p => CustomPlug.ForceGetWithComponent(profile, p));
			this.Definition = DefinitionStore.PlugSets[this.Hash];
		}
	}
}
