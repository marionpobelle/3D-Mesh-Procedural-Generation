using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator
{

    public static Texture2D TextureFromColourMap(Color[] colourMap, int width, int height){
        Texture2D texture = new Texture2D(width, height);
        //Makes it less blurry
        texture.filterMode = FilterMode.Point;
        //Fix the thing where we can see a bit of the other side of the map on the opposite side
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colourMap);
        texture.Apply();
        return texture;
    }

    public static Texture2D TextureFromHeightMap(float[,] heightMap){
        //Width and Height of the noisemap
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        //Set the color of each pixel
        Color[] colourMap = new Color[width * height];
        for(int y = 0; y < height; y++){
            for(int x = 0; x < width; x++){
                colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x,y]);
            }
        }
        return TextureFromColourMap(colourMap, width, height);
    }
    
}
