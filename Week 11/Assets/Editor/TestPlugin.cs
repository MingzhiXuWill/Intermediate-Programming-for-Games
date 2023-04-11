using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestPlugin : MonoBehaviour
{
    [MenuItem("MyMenu/Log Selected Transform Name")]
    static void LogSelectedTransformName()
    {
        Debug.Log("Selected Transform is on " + Selection.activeTransform.gameObject.name + ".");
    }
    [MenuItem("MyMenu/Log Selected Transform Name", true)]

    static bool ValidateLogSelectedTransformName()
    {
        return Selection.activeTransform != null;
    }
}
