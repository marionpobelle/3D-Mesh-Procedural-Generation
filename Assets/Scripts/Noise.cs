using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset){
        float[,] noiseMap = new float[mapWidth, mapHeight];

        //prng = pseudo random number generator
        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for(int i = 0; i < octaves; i++){
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if(scale <= 0) scale = 0.0001f;

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        for(int y = 0; y < mapHeight; y++){
            for(int x = 0; x < mapWidth; x++){

                //Frequency and amplitude
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight =0;


                //Loop to go through all of the octaves
                for(int i = 0; i < octaves; i++){
                    //Generate noise
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;
                    //*2 -1 so we can have negative values, implying we can have cliffs and caves
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) *2 -1;
                    noiseHeight += perlinValue * amplitude;

                    //range 0 to 1, decreases each octave
                    amplitude *= persistance;
                    //increases each octave lacunarity should be greater than 1
                    frequency *= lacunarity;
                }

                if(noiseHeight > maxNoiseHeight) maxNoiseHeight = noiseHeight;
                else if(noiseHeight < minNoiseHeight) minNoiseHeight = noiseHeight;

                noiseMap[x,y] = noiseHeight;
            }
        }
        //Normalize the noise map
        for(int y = 0; y < mapHeight; y++){
            for(int x = 0; x < mapWidth; x++){
                noiseMap[x,y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x,y]);
            }
        }
        return noiseMap;
    }










}
