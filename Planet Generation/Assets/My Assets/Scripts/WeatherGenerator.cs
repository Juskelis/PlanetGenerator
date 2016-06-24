using UnityEngine;
using System.Collections;
using LibNoise.Unity;
using LibNoise.Unity.Generator;
using LibNoise.Unity.Operator;
using LibNoiseGradient = LibNoise.Unity.Gradient;

public class WeatherGenerator : MonoBehaviour {

    private TerrainGenerator terrain;

    private Texture2D weatherImage;

    private Vector3 offset = Vector3.zero;

    private Billow billowModule;
    private Translate offsetModule;
    private Turbulence myModule;

    private Noise2D cloudMap;

	// Use this for initialization
	void Start () {
        terrain = GetComponent<TerrainGenerator>();
        Generate();
	}

    void Generate()
    {
        billowModule = new Billow();
        offsetModule = new Translate(0, 1, 0, billowModule);
        myModule = new Turbulence(1, offsetModule);
        
        Noise2D heightMapBuilder = new Noise2D(terrain.Width, terrain.Height, myModule);
        
        heightMapBuilder.GenerateSpherical(-90, 90, -180, 180);

        weatherImage = heightMapBuilder.GetTexture();
        weatherImage.Apply();

        GetComponent<LODMaterial>().OurMaterial.SetTexture("_Weather", weatherImage);
    }

    void Update()
    {
    }
}
