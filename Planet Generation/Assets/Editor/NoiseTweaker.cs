using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Tester))]
public class NoiseTweaker : Editor {

    public override void OnInspectorGUI()
    {
        Tester tester = (Tester) target;

        if (DrawDefaultInspector())
        {
            tester.Generate();
        }
    }
}
