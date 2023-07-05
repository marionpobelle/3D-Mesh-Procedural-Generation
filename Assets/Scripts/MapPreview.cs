using UnityEngine;

public class MapPreview : MonoBehaviour {

	//Map preview MeshFilter
	public MeshFilter meshFilter;
	//Map preview MeshRenderer
	public MeshRenderer meshRenderer;

	//Settings for the Mesh
	public MeshSettings meshSettings;
	//Settings for the HeightMap
	public HeightMapSettings heightMapSettings;

	//Transform of the plane to spawn as water
	[SerializeField]
    private Transform water;
    public Transform Water { get { return water; } }

	//Transform of the mesh used in the map preview
	public Transform meshTransform;

	//LOD for the preview in the EditorMode
	[Range(0,MeshSettings.numSupportedLODs-1)]
	public int editorPreviewLOD;

	//Boolean to indicate if we want the map to update with each change made in the editor
	public bool autoUpdate;

	//Item spawner to spawn the items to spread on the preview mesh
	public ItemSpawner spawnerInstance;

	/***
	 Generates the preview mesh in EditorMode.
	***/
	public void DrawMapInEditor() {
		HeightMap heightMap = HeightMapGenerator.GenerateHeightMap (meshSettings.numVertsPerLine, meshSettings.numVertsPerLine, heightMapSettings, Vector2.zero);
		heightMapSettings.UpdateBounds();
		DrawMesh (MeshGenerator.GenerateTerrainMesh (heightMap.values, editorPreviewLOD, meshSettings, heightMapSettings));
		DrawWaterInEditor(meshSettings);
		spawnerInstance.DrawItems(Vector2.zero, this.transform);
	}

	/***
	Destroys spawn items on the preview mesh. This is used to prevent items spawned before an update of the preview to survive through said update.
	- Doesn't destroy items when compiling the project again. Need to find a way to do that.
	***/
	public void NullifyChildren() {
		foreach (Transform child in this.transform) {
			DestroyImmediate(this.gameObject.transform.GetChild(0).gameObject);
		}
	}

	/***
	Redraw the map preview if mesh and heightMap settings values have been updated.
	***/
	void OnValuesUpdated() {
		if (!Application.isPlaying) {
			DrawMapInEditor ();
		}
	}

	/***
	Subscribes the update of the settings values if they are valid.
	***/
	void OnValidate() {
		if (meshSettings != null) {
			meshSettings.OnValuesUpdated -= OnValuesUpdated;
			meshSettings.OnValuesUpdated += OnValuesUpdated;
		}
		if (heightMapSettings != null) {
			heightMapSettings.OnValuesUpdated -= OnValuesUpdated;
			heightMapSettings.OnValuesUpdated += OnValuesUpdated;
		}
	}

	/***
	Draw the mesh in the map preview.
	***/
	public void DrawMesh(MeshData meshData) {
		meshFilter.sharedMesh = meshData.CreateMesh ();
		meshFilter.gameObject.SetActive(true);
	}

	/***
	Draw the water plane in the map preview.
	***/
	public void DrawWaterInEditor(MeshSettings meshSettings) {
		water.parent = meshTransform;
		float waterSideLength = meshSettings.meshWorldSize;
		water.localScale = new Vector3(waterSideLength / 10f, 1, waterSideLength / 10f);
		water.localPosition = new Vector3(meshTransform.localPosition.x, meshSettings.waterHeight, meshTransform.localPosition.z);
	}

}
