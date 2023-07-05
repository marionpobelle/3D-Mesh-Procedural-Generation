using UnityEngine;
using System.Collections.Generic;

public class TerrainGenerator : MonoBehaviour {

	//Viewer movement threshold for chunk update
	const float viewerMoveThresholdForChunkUpdate = 25f;
	//Square of above value
	const float sqrViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;

	//Index of the LOD for the collider
	public int colliderLODIndex;
	//Informations on LOD
	public LODInfo[] detailLevels;

	//Settings for the mesh
	public MeshSettings meshSettings;
	//Settings for the heightMap
	public HeightMapSettings heightMapSettings;

	//Transform of the player/viewer
	public Transform viewer;
	//The plane used for water
	[SerializeField]
	public GameObject water;

	//Material used for the terrain
	public Material mapMaterial;

	//Current player/viewer position
	Vector2 viewerPosition;
	//Old player/viewer position
	Vector2 viewerPositionOld;

	//Size of the mesh in the world
	float meshWorldSize;
	//Amounts of chunks visible in viewing distance
	int chunksVisibleInViewDst;

	//Chunks in the terrain
	Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
	//Chunks that are visible in the terrain
	List<TerrainChunk> visibleTerrainChunks = new List<TerrainChunk>();

	/***
	Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
	***/
	void Start() {
		float maxViewDst = detailLevels [detailLevels.Length - 1].visibleDstThreshold;
		meshWorldSize = meshSettings.meshWorldSize;
		chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / meshWorldSize);
		UpdateVisibleChunks ();
	}

	/***
	Update is called every frame, if the MonoBehaviour is enabled.
	***/
	void Update() {
		//We update the mesh collider with each player/viewer movement
		viewerPosition = new Vector2 (viewer.position.x, viewer.position.z);
		if (viewerPosition != viewerPositionOld) {
			foreach(TerrainChunk chunk in visibleTerrainChunks){
				chunk.UpdateCollisionMesh();
			}
		}
		//As well as the visible chunks
		if ((viewerPositionOld - viewerPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate) {
			viewerPositionOld = viewerPosition;
			UpdateVisibleChunks ();
		}
	}
	
	/***
	Updates the visible chunks.
	***/
	void UpdateVisibleChunks() {
		HashSet<Vector2> alreadyUpdatedChunkCoords = new HashSet<Vector2> ();
		for (int i = visibleTerrainChunks.Count-1; i >= 0; i--) {
			alreadyUpdatedChunkCoords.Add (visibleTerrainChunks [i].coord);
			visibleTerrainChunks [i].UpdateTerrainChunk ();
		}
		int currentChunkCoordX = Mathf.RoundToInt (viewerPosition.x / meshWorldSize);
		int currentChunkCoordY = Mathf.RoundToInt (viewerPosition.y / meshWorldSize);

		heightMapSettings.UpdateBounds();

		for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++) {
			for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++) {
				Vector2 viewedChunkCoord = new Vector2 (currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);
				if (!alreadyUpdatedChunkCoords.Contains (viewedChunkCoord)) {
					if (terrainChunkDictionary.ContainsKey (viewedChunkCoord)) {
						terrainChunkDictionary [viewedChunkCoord].UpdateTerrainChunk ();
					} else {
						TerrainChunk newChunk = new TerrainChunk (viewedChunkCoord, heightMapSettings, meshSettings, detailLevels, colliderLODIndex, transform, viewer, mapMaterial, water);
						terrainChunkDictionary.Add (viewedChunkCoord, newChunk);
						newChunk.onVisibilityChanged += OnTerrainChunkVisibilityChanged;
						newChunk.Load();
					}
				}
			}
		}
	}

	/***
	Adds or removes a chunk from the visible chunks list considering a change in its visibility.
	***/
	void OnTerrainChunkVisibilityChanged(TerrainChunk chunk, bool isVisible) {
		if (isVisible) {
			visibleTerrainChunks.Add (chunk);
		} else {
			visibleTerrainChunks.Remove (chunk);		
		}
	}

}

[System.Serializable]
public struct LODInfo {

	[Range(0,MeshSettings.numSupportedLODs-1)]
	public int lod;
	public float visibleDstThreshold;

	public float sqrVisibleDstThreshold {
		get {
			return visibleDstThreshold * visibleDstThreshold;
		}
	}
}
