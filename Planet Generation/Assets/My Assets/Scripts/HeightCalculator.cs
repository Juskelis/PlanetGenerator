using System;
using UnityEngine;
using System.Collections;

namespace PlanetGenerator
{
    public static class HeightCalculator
    {
        public enum NormalizeMode
        {
            Local,
            Global
        }

        private static int SYSTEM_RANDOM_RANGE = 100000;

        public static float[,] GenerateNoiseGrid(
            int width,
            int height,
            int seed = 0,
            float noiseScale = 100f,
            int octaves = 6,
            float persistance = 0.5f,
            float lacunarity = 2f,
            Vector2 offset = default(Vector2),
            NormalizeMode normalizeMode = NormalizeMode.Local
            )
        {
            noiseScale = (noiseScale <= 0) ? 1 : noiseScale;

            float maxPossibleHeight = 0;
            float amplitude = 1;
            float frequency = 1;

            System.Random prng = new System.Random(seed);
            Vector2[] octaveOffsets = new Vector2[octaves];
            for (int i = 0; i < octaves; i++)
            {
                float xOff = prng.Next(-SYSTEM_RANDOM_RANGE, SYSTEM_RANDOM_RANGE) + offset.x;
                float yOff = prng.Next(-SYSTEM_RANDOM_RANGE, SYSTEM_RANDOM_RANGE) - offset.y;
                octaveOffsets[i] = new Vector2(xOff, yOff);

                maxPossibleHeight += amplitude;

                amplitude *= persistance;

            }

            float[,] noiseGrid = new float[width, height];

            float minLocalNoiseHeight = float.MaxValue;
            float maxLocalNoiseHeight = float.MinValue;

            float halfWidth = width/2f;
            float halfheight = height/2f;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    amplitude = 1;
                    frequency = 1;

                    float noiseSum = 0;

                    for (int i = 0; i < octaves; i++)
                    {
                        float sampleX = (x - halfWidth + octaveOffsets[i].x)/noiseScale*frequency;
                        float sampleY = (y - halfheight + octaveOffsets[i].y)/noiseScale*frequency;

                        float noiseValue = Mathf.PerlinNoise(sampleX, sampleY)*2 - 1;
                        noiseSum += noiseValue*amplitude;

                        amplitude *= persistance;
                        frequency *= lacunarity;
                    }

                    minLocalNoiseHeight = Mathf.Min(minLocalNoiseHeight, noiseSum);
                    maxLocalNoiseHeight = Mathf.Max(maxLocalNoiseHeight, noiseSum);

                    noiseGrid[x, y] = noiseSum;
                }
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (normalizeMode == NormalizeMode.Local)
                    {
                        noiseGrid[x, y] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseGrid[x, y]);
                    }
                    else
                    {
                        float normalizedHeight = (noiseGrid[x, y] + 1)/(maxPossibleHeight);
                        noiseGrid[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);

                    }
                }
            }


            return noiseGrid;
        }

        public static float[,] GenerateSphericalNoiseGrid(
            int width,
            int height,
            int seed = 0,
            float noiseScale = 100f,
            int octaves = 6,
            float persistance = 0.5f,
            float lacunarity = 2f,
            Vector2 offset = default(Vector2)
            )
        {
            noiseScale = (noiseScale <= 0) ? 1 : noiseScale;

            float[,] noiseGrid = new float[width, height];

            float maxPossibleHeight = 0;
            float amplitude = 1;
            float frequency = 1;

            float minLocalNoiseHeight = float.MaxValue;
            float maxLocalNoiseHeight = float.MinValue;

            float theta = 0;
            float thetaStep = 360/(width);

            float azimuth = 0;
            float azimuthStep = 360/(height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    amplitude = 1;
                    frequency = 1;

                    float noiseSum = 0;

                    for (int i = 0; i < octaves; i++)
                    {
                        /*
                    float sampleX = theta * noiseScale * frequency;
                    float sampleY = azimuth * noiseScale * frequency;
                    */
                        ///*

                        float lat = y/height*180 - 90;
                        float lon = x/width*360 - 180;
                        float radius = Mathf.Cos(lat*Mathf.Deg2Rad);

                        float sampleX = (radius*Mathf.Cos(lon*Mathf.Deg2Rad) + 1)/noiseScale;
                        float sampleY = (radius*Mathf.Sin(lon*Mathf.Deg2Rad) + 1)/noiseScale;
                        //*/

                        float noiseValue = Mathf.PerlinNoise(sampleX, sampleY)*2 - 1;
                        noiseSum += noiseValue*amplitude;

                        amplitude *= persistance;
                        frequency *= lacunarity;
                    }

                    minLocalNoiseHeight = Mathf.Min(minLocalNoiseHeight, noiseSum);
                    maxLocalNoiseHeight = Mathf.Max(maxLocalNoiseHeight, noiseSum);

                    noiseGrid[x, y] = noiseSum;
                }
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    noiseGrid[x, y] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseGrid[x, y]);
                }
            }

            return noiseGrid;
            /*
        noiseScale = (noiseScale <= 0) ? 1 : noiseScale;

        float maxPossibleHeight = 0;
        float amplitude = 1;
        float frequency = 1;

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float xOff = prng.Next(-SYSTEM_RANDOM_RANGE, SYSTEM_RANDOM_RANGE) + offset.x;
            float yOff = prng.Next(-SYSTEM_RANDOM_RANGE, SYSTEM_RANDOM_RANGE) - offset.y;
            octaveOffsets[i] = new Vector2(xOff, yOff);

            maxPossibleHeight += amplitude;

            amplitude *= persistance;

        }

        float[,] noiseGrid = new float[width, height];

        float minLocalNoiseHeight = float.MaxValue;
        float maxLocalNoiseHeight = float.MinValue;

        float halfWidth = width / 2f;
        float halfheight = height / 2f;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                amplitude = 1;
                frequency = 1;

                float noiseSum = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = ((x - halfWidth + octaveOffsets[i].x) / width) / noiseScale * frequency;
                    float sampleY = ((y - halfheight + octaveOffsets[i].y) / height) / noiseScale * frequency;

                    float noiseValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseSum += noiseValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                minLocalNoiseHeight = Mathf.Min(minLocalNoiseHeight, noiseSum);
                maxLocalNoiseHeight = Mathf.Max(maxLocalNoiseHeight, noiseSum);

                noiseGrid[x, y] = noiseSum;
            }
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (normalizeMode == NormalizeMode.Local)
                {
                    noiseGrid[x, y] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseGrid[x, y]);
                }
                else
                {
                    float normalizedHeight = (noiseGrid[x, y] + 1) / (maxPossibleHeight);
                    noiseGrid[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);

                }
            }
        }


        return noiseGrid;
        */
        }

        // returns 0..1 value
        public static float GetValue(Vector2 location)
        {
            return Mathf.PerlinNoise(location.x, location.y);
        }
    }
}