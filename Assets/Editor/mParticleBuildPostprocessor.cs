using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace mParticle {

    public class BuildPostprocessor : MonoBehaviour
    {
        [PostProcessBuildAttribute(-1000)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuild)
        {
            UnityEngine.Debug.Log("Beginning postprocess: "+pathToBuild);

            string player = target.ToString() == "iOS" ? "iPhone" : target.ToString();
            string arguments = String.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\" \"{6}\"", pathToBuild,
                player, "", PlayerSettings.companyName, PlayerSettings.productName,
                PlayerSettings.defaultScreenWidth, PlayerSettings.defaultScreenHeight);

            UnityEngine.Debug.Log("Starting script: ruby " + Application.dataPath + "/Editor/PostprocessBuildPlayer_mParticle" + " " + arguments);

            Process process = new Process();
            process.StartInfo.FileName = "ruby";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.Arguments = "\"" + Application.dataPath + "/Editor/PostprocessBuildPlayer_mParticle\"" + " " + arguments;
            process.Start();

            // Synchronously read the standard output of the spawned process.
            StreamReader reader = process.StandardOutput;
            string output = reader.ReadToEnd();

            // Write the redirected output to this application's window.
            Console.WriteLine(output);

            process.WaitForExit();
            process.Close();

            UnityEngine.Debug.Log("Done postprocess");
        }
    }
}