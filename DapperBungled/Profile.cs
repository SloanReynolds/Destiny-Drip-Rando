using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BungieSharper.Entities;

namespace DapperBungled {
	public struct Profile : IEquatable<Profile> {
		public long ID { get; }
		public BungieMembershipType Type { get; }

		public Profile(long id, BungieMembershipType type = BungieMembershipType.TigerSteam) {
			ID = id;
			Type = type;
		}



		public override bool Equals(object? obj) {
			return obj is Profile profile && this.Equals(profile);
		}

		public bool Equals(Profile other) {
			return this.ID == other.ID &&
				   this.Type == other.Type;
		}

		public override int GetHashCode() {
			return HashCode.Combine(this.ID, this.Type);
		}

		public static bool operator ==(Profile left, Profile right) {
			return left.Equals(right);
		}

		public static bool operator !=(Profile left, Profile right) {
			return !(left == right);
		}
	}
}
