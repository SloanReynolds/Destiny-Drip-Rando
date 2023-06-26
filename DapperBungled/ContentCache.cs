using System.Collections.Concurrent;

namespace DapperBungled {
	public static class ContentCache {
		private static string _dir = "";
		private static ConcurrentDictionary<string, object> _locks = new();

		internal static void CreateDirectory(string downloadDir) {
			if (!Directory.Exists(_dir)) {
				Directory.CreateDirectory(_dir);
			}
		}

		public static string Retrieve(string urlPart, Action<string> callback = null) {
			if (string.IsNullOrWhiteSpace(urlPart))
				return "";

			string osPath = Path.Join(_dir, urlPart);
			if (!File.Exists(osPath)) {
				DownloadFile(urlPart);
			}
			if (callback != null) {
				callback.Invoke(osPath);
			}
			return osPath;
		}

		private static void DownloadFile(string urlPart) {
			DownloadFileTo(urlPart, Path.Join(_dir, urlPart));
		}

		private static void DownloadFileTo(string sourceUrlPart, string destPath) {
			using (var client = new HttpClient())
			using (var s = client.GetStreamAsync($"https://www.bungie.net{sourceUrlPart}").GetAwaiter().GetResult()) {
				string destDir = Path.GetDirectoryName(destPath);
				if (!Directory.Exists(destDir)) {
					Directory.CreateDirectory(destDir);
				}
				using (var fs = new FileStream(destPath, FileMode.OpenOrCreate))
					s.CopyTo(fs);
			}
		}

		public static async Task<string> AsyncRetrieve(string urlPart) {
			if (urlPart == null)
				return "";

			string osPath = Path.Join(_dir, urlPart);
			//lock (_locks.GetOrAdd(urlPart, new object())) {
				if (!File.Exists(osPath)) {
					await AsyncDownloadFile(urlPart);
					//    Below is a dirty hack; can't 'await' in a lock... lol fck deadlcks i no betr
					//AsyncDownloadFile(urlPart).GetAwaiter().GetResult();
				}
			//}
			return osPath;
		}

		internal static async Task AsyncDownloadFile(string urlPart) {
			await AsyncDownloadFileTo(urlPart, Path.Join(_dir, urlPart));
		}

		internal static async Task AsyncDownloadFileTo(string sourceUrlPart, string destPath) {
			using (var client = new HttpClient())
			using (var s = await client.GetStreamAsync($"https://www.bungie.net{sourceUrlPart}")) {
				string destDir = Path.GetDirectoryName(destPath);
				if (!Directory.Exists(destDir)) {
					Directory.CreateDirectory(destDir);
				}
				using (var fs = new FileStream(destPath, FileMode.OpenOrCreate))
					await s.CopyToAsync(fs);
			}
		}

		internal static void UpdateDownloadDir(string downloadDir) {
			_dir = Path.Join(downloadDir, "cache");
		}
	}
}
