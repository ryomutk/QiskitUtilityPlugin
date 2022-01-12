using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEditor;
using System.IO;

#if UNITY_EDITOR
[InitializeOnLoad]
public class AutoSetup
{
    static AutoSetup()
    {
        if (!Directory.Exists(Application.streamingAssetsPath + "/FlaskServer/build"))
        {
            Process p = new Process();
            p.StartInfo.FileName = System.Environment.GetEnvironmentVariable("ComSpec");
            p.StartInfo.Arguments = "/k python Assets/StreamingAssets/FlaskServer/setup.py build -b ./Assets/StreamingAssets/FlaskServer/build";
            p.Start();
            p.WaitForInputIdle();
            p.Kill();
            p.Close();
        }
    }

}
#endif