internal static class ExtensionMethods {
	private static Random _rng = new Random();

	public static T GetRandom<T>(this IEnumerable<T> plugs) {
		var plugArr = plugs.ToArray();
		return plugArr[_rng.Next(plugArr.Length)];
	}
}
