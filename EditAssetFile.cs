using System;
using MelonLoader;
using AssetsTools.NET;
using AssetsTools.NET.Extra;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace AudioCore
{
	public static class EditAssetFile
	{
		private static string DataPath = "tld_Data\\";
        private static string AssetFile = "globalgamemanagers";
        private static string PluginsPath = "Plugins\\";

        private static float[] m_DefaultContactOffset = {0.01f, 0.0075f, 0.005f, 0.0035f}; // float
        private static int[] m_DefaultSolverIterations = { 10, 10, 20, 30 };  // int
        private static int[] m_DefaultSolverVelocityIterations = { 5, 5, 10, 20 }; // int
        private static int[] m_DefaultMaxAngularSpeed = { 35, 35, 35, 35 }; // int
        private static bool[] m_EnableAdaptiveForce = { true, true, true, true }; // bool

     

        public static void CopyGlobalgamemanagersFile()
        {
            File.Copy(Path.Combine(DataPath, AssetFile), Path.Combine(DataPath, AssetFile + ".org"), true);

            MemoryStream memoryStream;
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AudioCore.Resources.globalgamemanagers");
            memoryStream = new MemoryStream((int)stream.Length);
            stream.CopyTo(memoryStream);

            File.WriteAllBytes(Path.Combine(DataPath, AssetFile), memoryStream.ToArray());
        }

		


        /*
        public static void SetLayerFields()
        {
            layerInfo = afi.file.GetAssetInfo(3);
            layerBaseField = am.GetBaseField(afi, layerInfo);

            try
            {               
               var layersArray = layerBaseField.Get("layers").Get("Array");
              
                layersArray.Children[3].Value.AsString = "Grabbing";
                layersArray.Children[6].Value.AsString = "HandPlayer"; 
                layersArray.Children[7].Value.AsString = "Hand";

                byte[] layerSection;

                using (MemoryStream memStream = new MemoryStream())
                using (AssetsFileWriter writer = new AssetsFileWriter(memStream))
                {
                    layerBaseField.Write(writer);
                    layerSection = memStream.ToArray();
                }
                // (long pathId, int classId, ushort monoScriptIndex, byte[] buffer)
                replacers.Add(new AssetsReplacerFromMemory(0, 3, 0x4E, layerSection));
            }
            catch (Exception ex)
            {
                MelonLogger.Msg($"An exception was encountered while editing layers: {ex.Message}");
            }
        }


        public static void SetPhysicsFields()
        {
            physicsInfo = afi.file.GetAssetInfo(10);
            physicsBaseField = am.GetBaseField(afi, physicsInfo);

            try
            {
                physicsBaseField.Get("m_DefaultContactOffset").Value.AsFloat = m_DefaultContactOffset[qualitySetting];
                physicsBaseField.Get("m_DefaultSolverIterations").Value.AsInt = m_DefaultSolverIterations[qualitySetting];
                physicsBaseField.Get("m_DefaultSolverVelocityIterations").Value.AsInt = m_DefaultSolverVelocityIterations[qualitySetting];
                physicsBaseField.Get("m_DefaultMaxAngularSpeed").Value.AsInt = m_DefaultMaxAngularSpeed[qualitySetting];
                physicsBaseField.Get("m_EnableAdaptiveForce").Value.AsBool = m_EnableAdaptiveForce[qualitySetting];

                byte[] physicsSection;

                using (MemoryStream memStream = new MemoryStream())
                using (AssetsFileWriter writer = new AssetsFileWriter(memStream))
                {
                    physicsBaseField.Write(writer);
                    physicsSection = memStream.ToArray();
                }

                replacers.Add(new AssetsReplacerFromMemory(0, 10, 0x37, physicsSection));
            }
            catch (Exception ex)
            {
                MelonLogger.Msg($"An exception was encountered: {ex.Message}");
            }
        }
        */



    }
}
