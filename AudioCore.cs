using MelonLoader;
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

            EditAssetFile.CopyGlobalgamemanagersFile();          
		}
	}
}