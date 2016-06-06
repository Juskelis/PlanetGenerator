using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
public class Tester : MonoBehaviour
{
    [Range(0,6)]
    [SerializeField] private int subdivisions = 0;
    [SerializeField] private float radius = 1f;

    private void Awake()
    {
        GetComponent<MeshFilter>().mesh = OctahedronSphereCreator.Create(subdivisions, radius);
    }

    private void onValidate()
    {
        if (subdivisions < 0) subdivisions = 0;
        if (radius < 1f) radius = 1f;
    }
}
