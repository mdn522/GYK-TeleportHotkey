using System.IO;
using Harmony;
using System.Reflection;
using UnityEngine;
using System.Collections.Generic;

namespace QuickTeleport {
	public class MainPatcher {
		public static void Patch() {
			HarmonyInstance harmony = HarmonyInstance.Create("com.mdn522.quickteleport.mod");
			harmony.PatchAll(Assembly.GetExecutingAssembly());
		}
	}

	public class Config {
		private static Options options_ = null;

		public class SinglePressKey {
			public KeyCode Key => key_;
			private KeyCode key_;
			private bool isPressed_ = false;
			public bool AlreadyPressed => isPressed_;

			public SinglePressKey(KeyCode key) {
				key_ = key;
			}
			public SinglePressKey(KeyCode key, bool alreadyPressed) {
				key_ = key;
				isPressed_ = true;
			}
			public void ChangeKey(KeyCode key) {
				key_ = key;
			}
			public virtual bool IsPressed() {
				if (Input.GetKey(Key)) {
					if (!isPressed_) {
						isPressed_ = true;
						return true;
					}
				}
				else {
					isPressed_ = false;
				}
				return false;
			}
		}

		public class ToggleKey : SinglePressKey {
			private bool toggled_ = false;
			public bool Toggled => toggled_;

			public ToggleKey(KeyCode key) : base(key) { }

			public bool IsToggled() {
				IsPressed();
				return toggled_;
			}

			public override bool IsPressed() {
				if (base.IsPressed()) {
					toggled_ = !toggled_;
					return true;
				}
				return false;
			}
		}

		public class SwitchKey : SinglePressKey {
			private int state_ = 0;
			private int stateNum_ = 2;
			public int State => state_;

			public SwitchKey(KeyCode key) : base(key) { }

			public SwitchKey(KeyCode key, int statenum) : base(key) {
				stateNum_ = statenum;
			}

			public override bool IsPressed() {
				if (base.IsPressed()) {
					state_ = ++state_ % stateNum_;
					return true;
				}
				return false;
			}
		}

		public class Options {
			public SinglePressKey ConfigReloadKey;// = new SinglePressKey(KeyCode.F5);
			public SinglePressKey DumpGDPointsKey = null;// = new SinglePressKey(KeyCode.F6);
			//public SinglePressKey TP_HomeInsideKey = new SinglePressKey(KeyCode.Keypad0);
			//public SinglePressKey TP_HomeOutsideKey = new SinglePressKey(KeyCode.Keypad1);
			//public SinglePressKey TP_GraveYardKey = new SinglePressKey(KeyCode.Keypad2);
			//public SinglePressKey TP_MorgueKey = new SinglePressKey(KeyCode.Keypad3);
			//public SinglePressKey TP_QuarryKey = new SinglePressKey(KeyCode.Keypad4);
			public Dictionary<string, SinglePressKey> ArbitraryGDPointKeys = new Dictionary<string, SinglePressKey>();
			public Dictionary<string, string> GDPointAliases = new Dictionary<string, string>();

			public Options() {
				ConfigReloadKey = new SinglePressKey(KeyCode.F5);
			}
			public Options(Options opts) {
				ConfigReloadKey = new SinglePressKey(KeyCode.F5, opts.ConfigReloadKey.AlreadyPressed);
			}
		}

		public static void Log(string line) {
			File.AppendAllText(@"./QMods/QuickTeleport/log.txt", line);
		}

		private static bool parseBool(string raw) {
			return raw == "1" || raw.ToLower() == "true";
		}
		private static float parseFloat(string raw, float _default) {
			float value = 0;
			if (float.TryParse(raw, out value)) {
				return value;
			}
			return _default;
		}
		private static float parseFloat(string raw, float _default, float threshold) {
			float value = parseFloat(raw, _default);
			if (value > threshold) {
				return value;
			}
			return _default;
		}
		private static float parsePositive(string raw, float _default) {
			return parseFloat(raw, _default, 0);
		}
		private static float parseNonNegative(string raw, float _default) {
			float value = parseFloat(raw, _default);
			return value < 0 ? 0 : value;
		}

		public static Options GetOptions() {
			return GetOptions(false);
		}
		public static Options GetOptions(bool forceReload) {
			if (options_ == null) {
				options_ = new Options();
			}
			else if (forceReload) {
				options_ = new Options(options_);
			} else {
				return options_;
			}

			string cfgPath = @"./QMods/QuickTeleport/config.txt";
			string aliasPath = @"./QMods/QuickTeleport/Alias.txt";

			options_.ArbitraryGDPointKeys.Clear();
			options_.GDPointAliases.Clear();

			if (File.Exists(cfgPath)) {
				string[] lines = File.ReadAllLines(cfgPath);
				foreach (string line in lines) {
					if (line.Length < 3 || line[0] == '#') {
						continue;
					}
					string[] pair = line.Split('=');
					if (pair.Length > 1) {
						string key = pair[0];
						string rawVal = pair[1];
						switch (key) {
							case "ConfigReloadKey":
								try {
									options_.ConfigReloadKey.ChangeKey(Enum<KeyCode>.Parse(rawVal));
								}
								catch { }
								break;
							case "DumpGDPointsKey":
								try {
									options_.DumpGDPointsKey.ChangeKey(Enum<KeyCode>.Parse(rawVal));
								}
								catch { }
								break;
							default:
								try {
									// TODO remove this IF CLAUSE as ArbitraryGDPointKeys is being cleared each time config is reloaded
									if (!options_.ArbitraryGDPointKeys.ContainsKey(key)) {
										options_.ArbitraryGDPointKeys[key] = new SinglePressKey(Enum<KeyCode>.Parse(rawVal));
									} else {
										options_.ArbitraryGDPointKeys[key].ChangeKey(Enum<KeyCode>.Parse(rawVal));
									}
								} catch { }
								break;
						}					
					}
				}
			}

			if (File.Exists(aliasPath)) {
				string[] lines = File.ReadAllLines(aliasPath);
				foreach (string line in lines) {
					if (line.Length < 3 || line[0] == '#') {
						continue;
					}
					string[] pair = line.Split('=');
					if (pair.Length > 1) {
						string key = pair[0];
						string rawVal = pair[1];
						try {
							options_.GDPointAliases[key] = rawVal;
						} catch { }
					}
				}
			}
			return options_;
		}
	}
}