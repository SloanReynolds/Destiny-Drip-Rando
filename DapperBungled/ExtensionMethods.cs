internal static class ExtensionMethods {
	private static Random _rng = new Random();

	public static T GetRandom<T>(this IEnumerable<T> plugs) {
		var plugArr = plugs.ToArray();
		return plugArr[_rng.Next(plugArr.Length)];
	}

	public static string GetRandom(this char[] choices, int count) {
		string newStr = "";
		for (int i = 0; i < count; i++) {
			newStr += choices[_rng.Next(choices.Length)];
		}
		return newStr;
	}
}
