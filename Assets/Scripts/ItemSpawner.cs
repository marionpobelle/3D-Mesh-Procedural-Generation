using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {

    //List of items to randomly spawn on the map
    public List <GameObject> itemsToSpread;
    //Spawn ratios for the items to spread on the map, in the same order
    public List <Vector2> itemsRatios;
    //Amount of items to spawn on one chunk
    public int numItemsToSpawn = 150;

    //Size of a chunk
    public float chunkSize;
    //Settings for the Mesh
    public MeshSettings meshSettings;
    //Settings for the HeightMap
    public HeightMapSettings heightMapSettings;

    //General height offset to burry items in the ground
    public float heightOffset = 0.1f;
    //Height offset to burry grass in the ground
    public float grassHeightOffset = 0.2f;
    //Height offset to burry trees in the ground
    public float treeHeightOffset = 0.2f;

    /***
    Spawns the items to spread on the map.
    ***/
    public void DrawItems(Vector2 origins, Transform parentObject) {
        if (meshSettings.useFlatShading) {
            this.chunkSize = MeshSettings.supportedChunkSizes[meshSettings.flatshadedChunkSizeIndex];
        } else {
            this.chunkSize = MeshSettings.supportedChunkSizes[meshSettings.chunkSizeIndex];
        }  
        for (int i = 0; i < numItemsToSpawn; i++) {
            GameObject item = PickItem();
            SpreadItem(item, origins, parentObject);
        }
    }

    /***
    Randomly selects an item in the list of items to spread.
    ***/
    GameObject PickItem() {
        float rand = Random.Range(0f,1f);
        string itemName = null;

        if (rand >= itemsRatios[0].x && rand <= itemsRatios[0].y) {
            itemName = "Grass0";
        }
        else if (rand > itemsRatios[1].x && rand <= itemsRatios[1].y) {
            itemName = "Grass1";
        }
        else if (rand > itemsRatios[2].x && rand <= itemsRatios[2].y) {
            itemName = "Grass2";     
        }
        else if (rand > itemsRatios[3].x && rand <= itemsRatios[3].y) {
            itemName = "PP_Sunflower_04";
        }
        else if (rand > itemsRatios[4].x && rand <= itemsRatios[4].y) {
            itemName = "PP_Daffodil_03";
        }
        else if (rand > itemsRatios[5].x && rand <= itemsRatios[5].y) {
            itemName = "PP_Tree_10";
        }
        else if (rand > itemsRatios[6].x && rand <= itemsRatios[6].y) {
            itemName = "PP_Tree_02";
        }
        else itemName = null;

        foreach (GameObject item in itemsToSpread) {
            if (item.name == itemName) {
                return item;
            } 
        }
        Debug.Log("Item to spawn not found !");
        return null;

    }

    /***
    Choses a random world position for a given item and spawns it on the map.
    ***/
    void SpreadItem(GameObject item, Vector2 origins, Transform parentObject) {
        if (item == null) Debug.Log("Item to spawn is null");
        else {
            float spread = (float)(chunkSize + 2)/2f;
            float randX = 0f;
            float randZ = 0f;
            float height = -1f;
            float bottomThreshhold = 0.07f * heightMapSettings.maxHeight;
            float topThreshhold = 0.14f * heightMapSettings.maxHeight;
            //Check if the height at the chosen random position is correct a.k.a. we don't want to spawn objects in water or on mountains if they don't belong there.
            while (height < bottomThreshhold || height > topThreshhold) {
                randX = Random.Range(origins.x-spread, origins.x+spread);
                randZ = Random.Range(origins.y-spread, origins.y+spread);
                //Raycast to check the height at x = randX,z = randZ)
                Vector3 rayOrigin = new Vector3(randX, heightMapSettings.maxHeight * 2, randZ);
                Vector3 rayDirection = new Vector3(0, -1, 0);
                RaycastHit hit;
                int layerMask = 1 << 3;
                layerMask = ~layerMask;

                if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity, layerMask)) {
                    height = hit.point.y;           
                }
            }
            //Initialize spawn position considering height offsets
            Vector3 randPosition = new Vector3(randX,height- heightOffset,randZ);
            if (item.name == "Grass0" || item.name == "Grass1" || item.name == "Grass2") {
                    randPosition = new Vector3(randX, height - grassHeightOffset, randZ);
            }
            if (item.name == "PP_Tree_10" || item.name == "PP_Tree_02") {
                    randPosition = new Vector3(randX, height - treeHeightOffset, randZ);
            }
            GameObject clone = Instantiate(item, randPosition, Quaternion.identity);
            clone.transform.parent = parentObject;
            clone.name = item.name;
        }
    }

}
