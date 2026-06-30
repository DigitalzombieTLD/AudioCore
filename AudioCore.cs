using AssetsTools.NET.Extra;
using AssetsTools.NET;
using MelonLoader;
using System.Reflection;

namespace AudioCore
{
    public class AudioCoreMain : MelonPlugin
    {
        public static string DataPath = "tld_Data\\";
        public static string AssetFile = "globalgamemanagers";
        public static string PluginsPath = "Plugins\\";

        public static bool enableAudio = true;

        public override void OnPreInitialization()
        {
            if (enableAudio)
            {
                MelonLogger.Msg("Attempting to enable Unity audio...");
            }
            else
            {
                MelonLogger.Msg("Attempting to disable Unity audio...");
            }

            try
            {
                AssetsManager am = new AssetsManager();
                AssetsFileInstance afi = am.LoadAssetsFile(Path.Combine(DataPath, AssetFile), false);

                if (!TryLoadIncludedClassPackage(am))
                {
                    MelonLogger.Msg("Could not load MelonLoader's classdata.tpk -- a tpk update may be required. Aborting.");
                    return;
                }

                am.LoadClassDatabaseFromPackage(afi.file.Metadata.UnityVersion);

                AssetFileInfo audioInfo = afi.file.GetAssetsOfType(AssetClassID.AudioManager)[0];
                AssetTypeValueField audioBaseField = am.GetBaseField(afi, audioInfo);

                if (audioBaseField.Get("m_DisableAudio").Value.AsBool == true && enableAudio == false)
                {
                    MelonLogger.Msg("Unity audio already disabled. Skipping ...");
                }
                else if (audioBaseField.Get("m_DisableAudio").Value.AsBool == false && enableAudio == true)
                {
                    MelonLogger.Msg("Unity audio already enabled. Skipping ...");
                }
                else
                {
                    audioBaseField.Get("m_DisableAudio").Value.AsBool = !enableAudio;

                    audioInfo.SetNewData(audioBaseField);

                    using (MemoryStream memStream = new MemoryStream())
                    using (AssetsFileWriter writer = new AssetsFileWriter(memStream))
                    {
                        afi.file.Write(writer, 0);
                        am.UnloadAllAssetsFiles();
                        File.WriteAllBytes(Path.Combine(DataPath, AssetFile), memStream.ToArray());
                    }

                    if (enableAudio)
                    {
                        MelonLogger.Msg("Unity audio successfully enabled!");
                    }
                    else
                    {
                        MelonLogger.Msg("Unity audio successfully disabled!");
                    }
                }
                am.UnloadAllAssetsFiles();               
            }
            catch (Exception ex)
            {
                MelonLogger.Msg($"An exception was encountered while attempting to toggle Unity audio: {ex.Message}");
            }
        }

        private static bool TryLoadIncludedClassPackage(AssetsManager am)
        {
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                string[] names;
                try { names = asm.GetManifestResourceNames(); }
                catch { continue; }

                string name = names.FirstOrDefault(n => n.EndsWith("classdata.tpk", StringComparison.OrdinalIgnoreCase));
                if (name == null) continue;

                using Stream s = asm.GetManifestResourceStream(name);
                MemoryStream ms = new MemoryStream();
                s.CopyTo(ms);
                ms.Position = 0;
                am.LoadClassPackage(ms);
                return true;
            }
            return false;
        }
    }
}