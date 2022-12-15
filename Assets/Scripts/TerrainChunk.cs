using UnityEngine;

public class TerrainChunk {

	//Threshold for collider generation
    const float colliderGenerationDistanceThreshold = 5;
	//Event for visibility change
    public event System.Action<TerrainChunk, bool> onVisibilityChanged;
	//Coordinates of the chunk
	public Vector2 coord;
	//Chunk itself
	GameObject meshObject;
	//Coordinates for the chunk center
	Vector2 sampleCentre;
	//Chunk bounds
	Bounds bounds;

	//MeshRenderer for the chunk
	MeshRenderer meshRenderer;
	//MeshFilter for the chunk
	MeshFilter meshFilter;
	//MeshCollider for the chunk
	MeshCollider meshCollider;

	//Detail levels for the chunk
	LODInfo[] detailLevels;
	//LOD meshes for the chunk
	LODMesh[] lodMeshes;
	//Index of the LOD that activates the chunk collider
	int colliderLODIndex;

	//HeightMap of the chunk
	HeightMap heightMap;
	//Boolean indicating if the chunk heightMap has been received
	bool heightMapReceived;
	//Index of the previous LOD
	int previousLODIndex = -1;
	//Boolean indicating if the collider has been set or not
	bool hasSetCollider;
	//Maximum view distance of the chunk
    float maxViewDst;

	//Settings for the heightMap
	HeightMapSettings heightMapSettings;
	//Settings for the mesh
	MeshSettings meshSettings;

	//Transform of the player/viewer
    Transform viewer;
	//Reference to the water plane
	GameObject water;
	//Reference to the item spawner
	ItemSpawner spawnerInstance;

	//Transform of the chunk
	Transform chunkTransform;
	//Water plane associated to the chunk itself
	GameObject waterPlaneAssociatedToChunk;

	public TerrainChunk(Vector2 coord, HeightMapSettings heightMapSettings, MeshSettings meshSettings, LODInfo[] detailLevels, int colliderLODIndex, Transform parent, Transform viewer, Material material, GameObject water) {
		this.coord = coord;
		this.detailLevels = detailLevels;
		this.colliderLODIndex = colliderLODIndex;
		this.heightMapSettings = heightMapSettings;
		this.meshSettings = meshSettings;
	    this.viewer = viewer;
		this.water = water;

		sampleCentre = coord * meshSettings.meshWorldSize / meshSettings.meshScale;
		spawnerInstance = GameObject.FindObjectOfType<ItemSpawner>();
		Vector2 position = coord * meshSettings.meshWorldSize;
		bounds = new Bounds(position, Vector2.one * meshSettings.meshWorldSize);

		meshObject = new GameObject("Terrain Chunk");
		this.chunkTransform = meshObject.transform;
		meshRenderer = meshObject.AddComponent<MeshRenderer>();
		meshFilter = meshObject.AddComponent<MeshFilter>();
		meshCollider = meshObject.AddComponent<MeshCollider>();
		meshRenderer.material = material;
		meshObject.transform.position = new Vector3(position.x, 0, position.y);
		meshObject.transform.parent = parent;
		SetVisible(false);

		lodMeshes = new LODMesh[detailLevels.Length];
		for (int i = 0; i < detailLevels.Length; i++) {
			lodMeshes[i] = new LODMesh(detailLevels[i].lod);
			lodMeshes[i].updateCallback += UpdateTerrainChunk;
			if (i == colliderLODIndex) {
				lodMeshes[i].updateCallback += UpdateCollisionMesh;
			}
		}
        maxViewDst = detailLevels[detailLevels.Length-1].visibleDstThreshold;
	}

	/***
	Loads the terrain chunk and draw its water plane.
	***/
    public void Load() {
		ThreadedDataRequester.RequestData(() => HeightMapGenerator.GenerateHeightMap (meshSettings.numVertsPerLine, meshSettings.numVertsPerLine, heightMapSettings, sampleCentre), OnHeightMapReceived);
		DrawWater(meshSettings);		
	}

	/***
	Initialize the chunk heightMap and updates the chunk.
	***/
	void OnHeightMapReceived(object heightMapObject) {
		this.heightMap = (HeightMap)heightMapObject;
		heightMapReceived = true;
		UpdateTerrainChunk ();
	}

	//World position of the player/viewer
    Vector2 viewerPosition {
        get {
            return new Vector2 (viewer.position.x, viewer.position.z);
        }
	}

	/***
	Updates the chunk.
	Considers visibility and LOD.
	***/
	public void UpdateTerrainChunk() {
		if (heightMapReceived) {
			float viewerDstFromNearestEdge = Mathf.Sqrt (bounds.SqrDistance (viewerPosition));

			bool wasVisible = IsVisible ();
			bool visible = viewerDstFromNearestEdge <= maxViewDst;

			if (visible) {
				int lodIndex = 0;

				for (int i = 0; i < detailLevels.Length - 1; i++) {
					if (viewerDstFromNearestEdge > detailLevels [i].visibleDstThreshold) {
						lodIndex = i + 1;
					} else {
						break;
					}
				}
				if (lodIndex != previousLODIndex) {
					LODMesh lodMesh = lodMeshes [lodIndex];
					if (lodMesh.hasMesh) {
						previousLODIndex = lodIndex;
						meshFilter.mesh = lodMesh.mesh;
					} else if (!lodMesh.hasRequestedMesh) {
						lodMesh.RequestMesh (heightMap, meshSettings, heightMapSettings);
					}
				}
			}
			if (wasVisible != visible) {
                SetVisible (visible);
				if(onVisibilityChanged != null){
                    onVisibilityChanged(this, visible);
                }
			}
		}
	}

	/***
	Updates the collision mesh considering the LOD and the collider generation threshold.
	Updates the water plane visibility as well.
	Draws the items to spread on the chunk at the same time as when the mesh collider is activated.
	***/
	public void UpdateCollisionMesh() {
		if (!hasSetCollider) {
			float sqrDstFromViewerToEdge = bounds.SqrDistance (viewerPosition);

			if (sqrDstFromViewerToEdge < detailLevels [colliderLODIndex].sqrVisibleDstThreshold) {
				if (!lodMeshes [colliderLODIndex].hasRequestedMesh) {
					lodMeshes [colliderLODIndex].RequestMesh (heightMap, meshSettings, heightMapSettings);
					waterPlaneAssociatedToChunk.SetActive(true);
				}
			}

			if (sqrDstFromViewerToEdge < colliderGenerationDistanceThreshold * colliderGenerationDistanceThreshold) {
				if (lodMeshes [colliderLODIndex].hasMesh) {
					meshCollider.sharedMesh = lodMeshes [colliderLODIndex].mesh;
					waterPlaneAssociatedToChunk.SetActive(true);
					hasSetCollider = true;
					spawnerInstance.DrawItems(sampleCentre, chunkTransform);
				}
			}
		}
	}

	/***
	Draws the water plane for the chunk.
	***/
	public void DrawWater(MeshSettings meshSettings) {
		Vector3 waterPosition = new Vector3(chunkTransform.localPosition.x, meshSettings.waterHeight, chunkTransform.localPosition.z);
		this.waterPlaneAssociatedToChunk = MonoBehaviour.Instantiate(water, waterPosition, Quaternion.identity);
        waterPlaneAssociatedToChunk.name = "WaterPlane";
		waterPlaneAssociatedToChunk.transform.parent = chunkTransform;
		float waterSideLength = meshSettings.meshWorldSize;
		waterPlaneAssociatedToChunk.transform.localScale = new Vector3(waterSideLength / 10f, 1, waterSideLength / 10f);

		waterPlaneAssociatedToChunk.SetActive(false);
		if (hasSetCollider) waterPlaneAssociatedToChunk.SetActive(true);
		UpdateWater();
	}

	/***
	Updates the water visibility.
	***/
	void UpdateWater() {
		if (!hasSetCollider) {
			float sqrDstFromViewerToEdge = bounds.SqrDistance (viewerPosition);

			if (sqrDstFromViewerToEdge < detailLevels [colliderLODIndex].sqrVisibleDstThreshold) {
				if (!lodMeshes [colliderLODIndex].hasRequestedMesh) {
					waterPlaneAssociatedToChunk.SetActive(true);
				}
			}

			if (sqrDstFromViewerToEdge < colliderGenerationDistanceThreshold * colliderGenerationDistanceThreshold) {
				if (lodMeshes [colliderLODIndex].hasMesh) {
					waterPlaneAssociatedToChunk.SetActive(true);
				}
			}
		}
	}

	/***
	Sets the visibility of the chunk.
	***/
	public void SetVisible(bool visible) {
		meshObject.SetActive (visible);
	}

	/***
	Checks if the chunk is visible.
	***/
	public bool IsVisible() {
		return meshObject.activeSelf;
	}

}

class LODMesh {

	public Mesh mesh;
	public bool hasRequestedMesh;
	public bool hasMesh;
	int lod;
	public event System.Action updateCallback;

	public LODMesh(int lod) {
		this.lod = lod;
			
	}

	void OnMeshDataReceived(object meshDataObject) {
		mesh = ((MeshData)meshDataObject).CreateMesh ();
		hasMesh = true;

		updateCallback ();
	}

	public void RequestMesh(HeightMap heightMap, MeshSettings meshSettings, HeightMapSettings heightMapSettings) {
		hasRequestedMesh = true;
		ThreadedDataRequester.RequestData(() => MeshGenerator.GenerateTerrainMesh(heightMap.values, lod, meshSettings, heightMapSettings), OnMeshDataReceived);
	}

}

