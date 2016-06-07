using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class LODMaterial : MonoBehaviour
{

    [SerializeField] private Material mat;
    private Material previousMat = null;

	// Use this for initialization
	void Update () {
	    if (previousMat != mat)
	    {
	        previousMat = mat;
	        foreach (var renderer in GetComponentsInChildren<MeshRenderer>())
	        {
	            renderer.material = mat;
	        }
	    }
	}
}
