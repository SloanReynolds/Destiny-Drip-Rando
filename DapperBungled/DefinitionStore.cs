using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BungieSharper.Entities.Destiny.Definitions;
using BungieSharper.Entities.Destiny.Definitions.Collectibles;
using BungieSharper.Entities.Destiny.Definitions.Sockets;

namespace DapperBungled {
	public class DefinitionStore {
		private static SQLiteConnection _conn;
		private static Dictionary<uint, DestinyCollectibleDefinition> _collectibles;
		private static Dictionary<uint, DestinyInventoryItemDefinition> _inventoryItems;
		private static Dictionary<uint, DestinyPlugSetDefinition> _plugSets;
		private static Dictionary<uint, DestinySocketCategoryDefinition> _socketCategories;

		public static IDictionary<uint, DestinyCollectibleDefinition> Collectibles => _collectibles;
		public static IDictionary<uint, DestinyInventoryItemDefinition> Items => _inventoryItems;
		public static IDictionary<uint, DestinyPlugSetDefinition> PlugSets => _plugSets;
		public static IDictionary<uint, DestinySocketCategoryDefinition> SocketCategories => _socketCategories;

		public static Dictionary<uint, T> LoadTable<T>(string tableName) {
			Dictionary<uint, T> newDict = new();
			SQLiteDataReader data = null;
			data = new SQLiteCommand($"SELECT * FROM {tableName}", _conn).ExecuteReader();

			while (data.Read()) {
				newDict.Add((uint)data.GetInt32(0), JsonSerializer.Deserialize<T>(data.GetString(1)));
			}

			return newDict;
		}

		internal static void LoadTables(SQLiteConnection conn) {
			_conn = conn;
			_collectibles = LoadTable<DestinyCollectibleDefinition>("DestinyCollectibleDefinition");
			_inventoryItems = LoadTable<DestinyInventoryItemDefinition>("DestinyInventoryItemDefinition");
			_plugSets = LoadTable<DestinyPlugSetDefinition>("DestinyPlugSetDefinition");
			_socketCategories = LoadTable<DestinySocketCategoryDefinition>("DestinySocketCategoryDefinition");
		}
	}
}
