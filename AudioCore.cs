using MelonLoader;
using System.IO;

namespace AudioCore
{
    public class AudioCoreMain : MelonPlugin
    {
        public static string DataPath = "tld_Data\\";
        public static string AssetFile = "globalgamemanagers";
        public static string PluginsPath = "Plugins\\";

        public static bool enableAudio = true;
        const int Offset = 62248;

        public override void OnPreInitialization()
        {
            if (Environment.GetCommandLineArgs().Contains("-noaudio") || Environment.GetCommandLineArgs().Contains("-disableaudio"))
            {
                enableAudio = false;
            }
            else if(Environment.GetCommandLineArgs().Contains("-audio") || Environment.GetCommandLineArgs().Contains("-enableaudio"))
            {
                enableAudio = true;
            }
            else
            {
                enableAudio = true;
            }

            string StateStr1 = enableAudio ? "enable" : "disable";
            string StateStr2 = enableAudio ? "enabled" : "disabled";

            MelonLogger.Msg($"Attempting to {StateStr1} Unity audio...");

            try
            {
                string GlobalManagerPath = Path.Combine(DataPath, AssetFile);

                if (File.Exists(GlobalManagerPath))
                {
                    MelonLogger.Msg($"{AssetFile} Found, prepare to patch...");
                }
                else
                {
                    MelonLogger.Msg(System.ConsoleColor.Red, $"{AssetFile} not found, patching aborted.");
                    return;
                }

                byte[] data = File.ReadAllBytes(GlobalManagerPath);

                if((data[Offset] == 0x01 && !enableAudio) || (data[Offset] == 0x00 && enableAudio))
                {
                    MelonLogger.Msg(System.ConsoleColor.Green, $"Audio is already {StateStr2}");
                    return;
                }

                data[Offset] = enableAudio ? (byte)0x00 : (byte)0x01;

                File.WriteAllBytes(GlobalManagerPath, data);

                MelonLogger.Msg(System.ConsoleColor.Green, $"{AssetFile} patching compleated! Audio now is {StateStr2}.");
            }
            catch (Exception ex)
            {
                MelonLogger.Msg($"An exception was encountered while attempting to toggle Unity audio: {ex.Message}");
            }
        }
    }
}