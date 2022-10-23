using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer textureRender;

    public void DrawNoiseMap(float [,] noiseMap){
        //Width and Height of the noisemap
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);
        //Create 2D texture
        Texture2D texture = new Texture2D(width, height);
        //Set the color of each pixel
        Color[] colourMap = new Color[width * height];
        for(int y = 0; y < height; y++){
            for(int x = 0; x < width; x++){
                colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x,y]);
            }
        }
        //Apply colors to texture
        texture.SetPixels(colourMap);
        texture.Apply();

        //Apply texture to texture renderer
        textureRender.sharedMaterial.mainTexture = texture;
        textureRender.transform.localScale = new Vector3(width, 1, height);
    }
}
