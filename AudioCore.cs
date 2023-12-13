using AssetsTools.NET.Extra;
using AssetsTools.NET;
using MelonLoader;

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
                MelonUtils.LoadIncludedClassPackage(am);
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

                    List<AssetsReplacer> reps = new List<AssetsReplacer>
                    {
                    new AssetsReplacerFromMemory(afi.file, audioInfo, audioBaseField)
                    };

                    using (MemoryStream memStream = new MemoryStream())
                    using (AssetsFileWriter writer = new AssetsFileWriter(memStream))
                    {
                        afi.file.Write(writer, 0, reps);
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
    }
}