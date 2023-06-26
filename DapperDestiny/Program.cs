using DapperBungled;

namespace DapperDestiny {
	internal static class Program {
		public static readonly Profile Profile = new Profile(4611686018467365389);
		private static AccountData _accountData;

		public static AccountData AccountData => _accountData;

		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			OAuth.SetContext(new FormsContext());
			ApiCallsEZ.InitializeSQLite().GetAwaiter().GetResult();
			_accountData = new AccountData(Profile);
			_accountData.LoadProfile_ForPlugSets();

			// To customize application configuration such as set high DPI settings or default font,
			// see https://aka.ms/applicationconfiguration.
			ApplicationConfiguration.Initialize();
			Application.Run(new Main());
		}
	}
}