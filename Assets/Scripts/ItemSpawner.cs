using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemSpawner : MonoBehaviour
{

    public List <GameObject> itemsToSpread;
    public int numItemsToSpawn = 10;

    public float chunkSize;

    //public Transform itemsHolder;

    public MeshSettings meshSettings;

    public void DrawItems(Vector2 origins, Transform parentObject){
        //could put this in a Start or Awake
        if(meshSettings.useFlatShading){
            this.chunkSize = MeshSettings.supportedChunkSizes[meshSettings.flatshadedChunkSizeIndex];
        }else{
            this.chunkSize = MeshSettings.supportedChunkSizes[meshSettings.chunkSizeIndex];
        }  
        //
        for(int i = 0; i < numItemsToSpawn; i++){
            GameObject item = PickItem();
            SpreadItem(item, origins, parentObject);
        }
    }

    //Pick an item randomly in itemsToSpread considering frequency
    GameObject PickItem(){
        float rand = Random.Range(0f,1f);
        string itemName = null;
        if(rand >= 0 && rand <= 0.5){
            if(rand >= 0 && rand <= 0.17){
                itemName = "Grass0";
            }
            else if(rand > 0.17 && rand <= 0.33){
                itemName = "Grass1";
            }
            else if(rand > 0.33 && rand <= 0.5){
                itemName = "Grass2";
            }
        }else if(rand > 0.5 && rand <= 0.6){
            itemName = "PP_Sunflower_04";
        }
        else if(rand > 0.6 && rand <= 0.7){
            itemName = "PP_Daffodil_03";
        }
        else if(rand > 0.7 && rand <= 0.8){
            itemName = "PP_Tree_10";
        }
        else if(rand > 0.8 && rand <= 0.9){
            itemName = "PP_Tree_02";
        }
        else if(rand > 0.9 && rand <= 1){
            itemName = "Log";
        }
        else itemName = null;

        foreach(GameObject item in itemsToSpread){
            if (item.name == itemName){
                return item;
            } 
        }
        Debug.Log("Item to spawn not found !");
        return null;

    }

    //spawn the item on a random position
    //gotta check the height Y at the X Z selected
    void SpreadItem(GameObject item, Vector2 origins, Transform parentObject){
        if(item == null) Debug.Log("Item to spawn is null");
        else{
            float spread = (float)(chunkSize + 2)/2f;
            //not gonna work if center of chunk isn't the origin
            float randX = Random.Range(origins.x-spread, origins.x+spread);
            float randZ = Random.Range(origins.y-spread, origins.y+spread);

            //Height things in MeshGenerator
            //float height = heightMap [randX, randZ];
            //float normalizedHeight = Mathf.InverseLerp(heightMapSettings.minHeight, heightMapSettings.maxHeight, height);

            Vector3 randPosition = new Vector3(randX,0,randZ);
            GameObject clone = Instantiate(item, randPosition, Quaternion.identity);
            //clone.transform.parent = itemsHolder;
            clone.transform.parent = parentObject;
            clone.name = item.name;
        }
    }
}
