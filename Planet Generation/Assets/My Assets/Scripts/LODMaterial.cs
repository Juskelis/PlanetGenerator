using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class LODMaterial : MonoBehaviour
{

    [SerializeField] private Material mat;
    public Material OurMaterial
    {
        get { return mat; }
        set
        {
            previousMat = null;
            mat = value;
        }
    }
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
