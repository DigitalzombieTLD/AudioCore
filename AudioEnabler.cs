using System;
using MelonLoader;
using AssetsTools.NET;
using AssetsTools.NET.Extra;
using System.IO;
using System.Collections.Generic;

namespace AudioCore
{
	public class Audio_Enabler_Main
	{
		public static bool currentAudioSettings;
		public static string DataPath = "tld_Data\\";
		public static string AssetFile = "globalgamemanagers";
		public static string PluginsPath = "Plugins\\";

		public static AssetsManager am;
		public static AssetsFileInstance afi;

		public static AssetFileInfo audioInfo;
		public static AssetTypeValueField audioBaseField;

		public static bool CheckAudio()
		{
			bool field = audioBaseField.Get("m_DisableAudio").AsBool;
			return field;
		}

		public static void LoadGlobalgamemanagers()
		{
			am = new AssetsManager();
			afi = am.LoadAssetsFile(Path.Combine(DataPath, AssetFile), false);
			MelonUtils.LoadIncludedClassPackage(am);
			am.LoadClassDatabaseFromPackage(afi.file.Metadata.UnityVersion);

			audioInfo = afi.file.GetAssetInfo(4);
			audioBaseField = am.GetBaseField(afi, audioInfo);
		}

		public static void DisableUnityAudio(bool choice)
		{
			if(choice)
			{
				MelonLogger.Msg("Attempting to disable Unity audio...");
			}
			else
			{
				MelonLogger.Msg("Attempting to enable Unity audio...");
			}			

			try
			{	
				audioBaseField.Get("m_DisableAudio").Value.AsBool = choice;

				byte[] audioAsset;
				using (MemoryStream memStream = new MemoryStream())
				using (AssetsFileWriter writer = new AssetsFileWriter(memStream))
				{					
					audioBaseField.Write(writer);
					audioAsset = memStream.ToArray();
				}
				List<AssetsReplacer> rep = new List<AssetsReplacer>() { new AssetsReplacerFromMemory(0, 4, 0x0B, 0xFFFF, audioAsset) };
				using (MemoryStream memStream = new MemoryStream())
				using (AssetsFileWriter writer = new AssetsFileWriter(memStream))
				{
					afi.file.Write(writer, 0, rep);
					afi.file.Close();
					File.WriteAllBytes(Path.Combine(DataPath, AssetFile), memStream.ToArray());
				}

				if (choice)
				{
					MelonLogger.Msg("Unity audio disabled!");
				}
				else
				{
					MelonLogger.Msg("Unity audio enabled!");
				}
			}
			catch (Exception ex)
			{
				MelonLogger.Msg($"An exception was encountered while attempting to toggle Unity audio: {ex.Message}");
			}
		}
	}
}
