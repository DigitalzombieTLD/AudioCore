﻿using MelonLoader;
using UnityEngine;
using Il2CppInterop;
using Il2CppInterop.Runtime.Injection;

namespace AudioCore
{
	public class AudioCoreMain : MelonPlugin
	{
		public static bool audioEnabled;

		public override void OnPreInitialization() // runs before game initialization.
		{
			MelonLogger.Msg("Audio preInitialization ...");

			Audio_Enabler_Main.LoadGlobalgamemanagers();

			if (!Audio_Enabler_Main.CheckAudio())
			{
				MelonLogger.Msg("Unity audio already enabled ... skipping");
			}
			else
			{
				Audio_Enabler_Main.DisableUnityAudio(false);
			}

			Audio_Enabler_Main.am.UnloadAllAssetsFiles();
		}
	}
}