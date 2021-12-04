using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Harmony;
using UnityEngine;
using static QuickTeleport.Config;

namespace QuickTeleport {
	[HarmonyPatch(typeof(EnvironmentEngine))]
	[HarmonyPatch("Update")]
	class EnvironmentEngine_Update_Patch {
		[HarmonyPrefix]
		static bool Prefix(EnvironmentEngine __instance) {
			if (MainGame.game_starting || MainGame.paused || !MainGame.game_started || __instance.IsTimeStopped()) {
				return true;
			}
			Config.Options opts = Config.GetOptions();
			if (opts.ConfigReloadKey.IsPressed()) {
				Config.GetOptions(true);
				EffectBubblesManager.ShowImmediately(MainGame.me.player.pos3, "QuickTeleport configuration reloaded");
		    } else if (opts.DumpGDPointsKey != null && opts.DumpGDPointsKey.IsPressed()) {

				Helper.Log("Dumping GD Points", false);
				Helper.Log("-------------------------------------", false);
				foreach (GDPoint gd_point in WorldMap.gd_points) {
					Helper.Log(string.Format("TAG: \"{0}\"; DISABLED: {1}", gd_point.gd_tag, gd_point.IsDisabled().ToString()), false);
				}

				EffectBubblesManager.ShowImmediately(MainGame.me.player.pos3, "Dumped GD Points to Log.txt");
			} else {
				foreach (KeyValuePair<string, SinglePressKey> kvp in opts.ArbitraryGDPointKeys.ToArray()) {
					string gd_point = kvp.Key;

					if (opts.GDPointAliases.ContainsKey(gd_point)) {
						gd_point = opts.GDPointAliases[gd_point];
					}

					if (kvp.Value.IsPressed()) {
						MainGame.me.player.TeleportToGDPoint(gd_point);
					}
				}
			}
			return true;
		}
	}
}