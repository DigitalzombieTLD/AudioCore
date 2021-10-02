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
		public static string ClassDatabase = "AudioCoreData.tpk";

		public static AssetsManager am;
		public static AssetsFileInstance afi;

		public static AssetFileInfoEx audioInfo;
		public static AssetTypeInstance audioAti;
		public static AssetTypeValueField audioBaseField;

		public static bool CheckAudio()
		{
			bool field = audioBaseField.Get("m_DisableAudio").GetValue().AsBool(); ;
			return field;
		}

		public static void LoadGlobalgamemanagers()
		{
			am = new AssetsManager();
			afi = am.LoadAssetsFile(Path.Combine(DataPath, AssetFile), false);
			am.LoadClassPackage(Path.Combine(PluginsPath, ClassDatabase));
			am.LoadClassDatabaseFromPackage(afi.file.typeTree.unityVersion);

			audioInfo = afi.table.GetAssetInfo(4);
			audioAti = am.GetTypeInstance(afi.file, audioInfo);
			audioBaseField = audioAti.GetBaseField();			
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
				audioBaseField.Get("m_DisableAudio").GetValue().Set(choice);

				byte[] audioAsset;
				using (MemoryStream memStream = new MemoryStream())
				using (AssetsFileWriter writer = new AssetsFileWriter(memStream))
				{
					writer.bigEndian = false;
					audioBaseField.Write(writer);
					audioAsset = memStream.ToArray();
				}
				List<AssetsReplacer> rep = new List<AssetsReplacer>() { new AssetsReplacerFromMemory(0, 4, 0x0B, 0xFFFF, audioAsset) };
				using (MemoryStream memStream = new MemoryStream())
				using (AssetsFileWriter writer = new AssetsFileWriter(memStream))
				{
					afi.file.Write(writer, 0, rep, 0);
					afi.stream.Close();
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
