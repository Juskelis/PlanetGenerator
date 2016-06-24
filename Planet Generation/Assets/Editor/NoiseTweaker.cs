using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TerrainGenerator))]
public class NoiseTweaker : Editor {

    public override void OnInspectorGUI()
    {
        TerrainGenerator tester = (TerrainGenerator) target;

        if (DrawDefaultInspector())
        {
            tester.Generate();
        }
    }
}
