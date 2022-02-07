using UnityEngine;
using System.Collections;
using UnityEditor;

public class SetupWizard:ScriptableWizard
{
    [MenuItem("Setup/Setup")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<SetupWizard>("Setup","Setup");
    }

    void OnWizardCreate()
    {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = System.Environment.GetEnvironmentVariable("ComSpec");
            //p.StartInfo.Arguments = "/k python Assets/StreamingAssets/FlaskServer/setup.py build -b ./Assets/StreamingAssets/FlaskServer/build";
            p.StartInfo.Arguments = "/k pip install -r Assets/StreamingAssets/FlaskServer/requirements.txt";
            p.Start();
            p.WaitForInputIdle();
            p.Kill();
            p.Close();
    }
}