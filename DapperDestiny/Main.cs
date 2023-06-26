using System.Diagnostics;
using System.Windows.Forms;
using BungieSharper.Client;
using BungieSharper.Entities.Destiny.HistoricalStats.Definitions;
using BungieSharper.Entities.Exceptions;
using DapperBungled;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DapperDestiny {
	public partial class Main : Form {
		private static Main _instance;

		internal static void SetToolTip(Control control, string message) {
			if (_instance == null || control == null)
				return;

			_instance.toolTip1.SetToolTip(control, message);
		}


		//private const uint ORBIT_ACTIVITY_HASH = 82913930;
		private const DestinyActivityModeType SOCIAL_ACTIVITY_TYPE = DestinyActivityModeType.Social;
		private const int SLEEP_TIME_MS = 15 * 1000;

		private long _previousCharacterId = 0;
		private uint _previousActivityHash = 0;
		private enum _SkipType {
			None,
			SameActivity,
			InvalidActivity
		}
		private _SkipType _skipType = _SkipType.None;

		private bool _isLooping = false;
		private CancellationTokenSource _loopCTS = new CancellationTokenSource();
		private bool _isRandomizing = false;

		public Main() {
			InitializeComponent();
			textBox1.Controls.Add(pictureBox1);
			//guardianGear.SetToolTip(toolTip1);

			_instance = this;
		}

		private void Main_Load(object sender, EventArgs e) {
			textBox1.Text = "";

			guardianGear.SetCharacter(Program.AccountData.Characters.Values.First());
			Program.AccountData.OnCharacterEquipmentUpdated +=
				(chr) => {
					Task.Run(() => {
						if (chr == guardianGear.CurrentCharacter)
							guardianGear.UpdateEquipment();
					});
				};
		}

		private void button2_Click(object sender, EventArgs e) {
			DisableButton2();

			Task.Run(_BeginRandomization);
		}

		private void button1_Click(object sender, EventArgs e) {
			if (_isLooping) {
				_StopLooping();
			} else {
				DisableButton2();

				_StartLooping();
			}
		}

		private void _StartLooping() {
			button1.Text = "Stop Randomizing";

			_loopCTS = new CancellationTokenSource();

			Task.Run(() => {
				_RandoOnRepeat(_loopCTS.Token);
			});

			_WriteLine($"Looping! Attempting every {SLEEP_TIME_MS / 1000} seconds.");
			_isLooping = true;
		}

		private void _StopLooping() {
			button1.Text = "Start Randomizing";

			_loopCTS.Cancel();

			_WriteLine("No longer looping.");
			_isLooping = false;
		}

		private async void _RandoOnRepeat(CancellationToken cancelToken) {
			LoopTaskData ltd = new();

			while (!cancelToken.IsCancellationRequested) {
				try {
					await _BeginRandomization(true, ltd);
				} catch (Exception ex) {
					_WriteLine($"Exception while trying: {ltd.GetLastTaskString()}");
					_WriteLine(ex.Message);
					_ShowErrorPic();
					//_BubbleException(ex);
				}
				Thread.Sleep(SLEEP_TIME_MS);
			}
		}

		private void _ShowErrorPic() {
			if (InvokeRequired) { Invoke(_ShowErrorPic); return; };

			pictureBox1.Show();
		}

		private void _BubbleException(Exception ex) {
			if (InvokeRequired) { Invoke(_BubbleException, ex); }

			throw ex;
		}

		private void EnableButton2() {
			if (InvokeRequired) { Invoke(EnableButton2); }

			button2.Enabled = true;
		}

		private void DisableButton2() {
			if (InvokeRequired) { Invoke(DisableButton2); }

			button2.Enabled = false;
		}

		private void _WaitAndLockRandomizer() {
			if (_isRandomizing) {
				Thread.Sleep(1000);
			}
			_LockRandomizer();
		}

		private void _LockRandomizer() {
			DisableButton2();
			_isRandomizing = true;
		}

		private void _UnlockRandomizer() {
			EnableButton2();
			_isRandomizing = false;
		}

		private async Task _BeginRandomization() {
			await _BeginRandomization(false, new LoopTaskData());
		}

		private async Task _BeginRandomization(bool isFromLooping, LoopTaskData ltd) {
			_WaitAndLockRandomizer();

			await _RandomizeIfValid(isFromLooping, ltd);

			_UnlockRandomizer();
		}

		private async Task _RandomizeIfValid(bool isFromLooping, LoopTaskData ltd) {
			var profile = Program.Profile;
			var data = Program.AccountData;
			data.UpdateData();
			var character = data.Characters.Values.OrderByDescending(chr => chr.LastPlayed).First();

			if (_previousCharacterId != character.ID) {
				_previousActivityHash = 0;
				_previousCharacterId = character.ID;
			}

			//Valid Activities?

			//SAME activity?
			if (isFromLooping && character.ActivityHash == _previousActivityHash) {
				//We are in the same activity as last time

				if (_skipType != _SkipType.SameActivity) {
					_skipType = _SkipType.SameActivity;
				}

				return;
			}

			if (_previousActivityHash != character.ActivityHash) {
				_WriteLine($"Activity Changed: ([{character.ActivityType}]{character.ActivityHash})");
				_previousActivityHash = character.ActivityHash;
			}

			if (false && (character.ActivityType != null && character.ActivityType != SOCIAL_ACTIVITY_TYPE)) {
				//We aren't in a social space
				if (!isFromLooping) { //Tell the click-happy loser that he can't do that
					_WriteLine($"Equipment changes disabled in current Activity.");
				}

				if (_skipType != _SkipType.InvalidActivity) {
					_skipType = _SkipType.InvalidActivity;
				}

				return;
			}

			//Made it past the skip conditions. Hooray!
			await _RandomizeEquipmentCosmetics(character, ltd);
		}

		private async Task _RandomizeEquipmentCosmetics(CustomCharacter character, LoopTaskData ltd) {
			var profile = Program.Profile;
			var data = Program.AccountData;
			List<Task> apiTasks = new();

			_WriteLine($"Randomizing... ([{character.ActivityType}]{character.ActivityHash})");
			_skipType = _SkipType.None;

			foreach (var item in character.Equipment.Values) {
				foreach (var socket in item.SocketPlugs.Where(csp => csp.Category != null && csp.Category.DisplayProperties.Name.Contains("COSMETIC")).OrderByDescending(sp => sp.Index)) {
					if (item.InstanceID == null) { continue; }
					var profileFilteredWhiteList
						= data.ProfilePlugs.Where(pp => pp.IsReady && socket.HashWhiteList.Contains(pp.Hash));

					if (!profileFilteredWhiteList.Select(cp => cp.Hash).Contains(socket.Definition.SingleInitialItemHash)) {
						profileFilteredWhiteList = profileFilteredWhiteList.Prepend(CustomPlug.ForceGet(profile, socket.Definition.SingleInitialItemHash));
					}

					var randomPlug = profileFilteredWhiteList.GetRandom();
					ltd.SetCurrentTaskData(item, randomPlug, socket);
					if (randomPlug != socket.Plug && randomPlug.Hash != socket.Plug.Hash) {
						apiTasks.Add(_InsertPlug(character, item, socket, randomPlug));
					}
				}
			}
			await Task.WhenAll(apiTasks);
			_WriteLine("Success!");
		}

		private async Task _InsertPlug(CustomCharacter character, CustomItem item, CustomItemSocket socket, CustomPlug randomPlug) {
			bool doUpdateGear = true;
			try {
				await ApiCallsEZ.InsertPlugFree(character, item, socket, randomPlug);
			} catch (BungieApiNoRetryException ex) {
				if (!_HandleExceptions_BungieApiNoRetry(ex, out doUpdateGear)) {
					throw ex;
				}

				return;
			} finally {
				if (doUpdateGear)
					guardianGear.UpdatePiece(item);
			}
		}

		private bool _HandleExceptions_BungieApiNoRetry(BungieApiNoRetryException ex, out bool doUpdateGearAfter) {
			doUpdateGearAfter = false;
			switch (ex.ApiResponseMsg.ErrorCode) {
				case PlatformErrorCodes.DestinySocketAlreadyHasPlug:
					_WriteLine("DestinySocketAlreadyHasPlug");
					doUpdateGearAfter = true;
					return true;
				case PlatformErrorCodes.DestinyCharacterNotInTower:
					_WriteLine("Stopped rando; you're busy silly!");
					return true;
				default:
					return false;
			}
		}

		private void _WriteLine(string message) {
			if (InvokeRequired) { Invoke(_WriteLine, message); return; }

			Debug.WriteLine(message);
			textBox1.AppendText(message + Environment.NewLine);
		}

		private void pictureBox1_Click(object sender, EventArgs e) {
			pictureBox1.Hide();
		}
	}
}