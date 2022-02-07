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
    static bool ready = false;
    static AutoSetup()
    {
        if (!ready)
        {

        }
    }

}
#endif