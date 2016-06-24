using UnityEngine;
using System.Collections;
using LibNoise.Unity;
using LibNoise.Unity.Generator;
using PlanetGenerator;
using LibNoiseGradient = LibNoise.Unity.Gradient;
using UnityGradient = UnityEngine.Gradient;

public class TerrainGenerator : MonoBehaviour
{
    [Range(1, 9)]
    [SerializeField]
    private int power = 1;

    private int width = 512;
    public int Width { get { return width; } }

    private int height = 256;
    public int Height { get { return height; } }

    [SerializeField]
    private UnityGradient gradient;

    [SerializeField]
    private float frequency = 2f;

    [SerializeField]
    private float lacunarity = 0.5f;

    [SerializeField]
    private float persistence = 0.5f;

    [SerializeField]
    private int octaves = 6;

    [SerializeField]
    private int seed;

    [SerializeField]
    private QualityMode quality;

    [SerializeField]
    private FilterMode imageFilterMode;
    [SerializeField]
    private TextureWrapMode imageWrapMode;


    private float west = -180;
    private float east = 180;
    private float north = 90;
    private float south = -90;

    private Texture2D image;

    void Start()
    {
        Generate();
    }

    public void Generate()
    {
        ModuleBase myModule = new Perlin(frequency, lacunarity, persistence, octaves, seed, quality);

        Noise2D heightMapBuilder = new Noise2D(width, height, myModule);

        heightMapBuilder.GenerateSpherical(south, north, west, east);

        LibNoiseGradient g = new LibNoiseGradient(gradient.colorKeys[0].color);
        foreach (var col in gradient.colorKeys)
        {
            g[col.time] = col.color;
        }

        image = heightMapBuilder.GetTexture(g);
        image.filterMode = imageFilterMode;
        image.wrapMode = imageWrapMode;
        image.Apply();

        GetComponent<LODMaterial>().OurMaterial.mainTexture = image;
    }

    void OnValidate()
    {
        width = 1 << power;
        height = 1 << (power - 1);
    }
}
