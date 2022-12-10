using UnityEngine;
using System.Collections;

public class MapPreview : MonoBehaviour {

	public Renderer textureRender;
	public MeshFilter meshFilter;
	public MeshRenderer meshRenderer;

	public enum DrawMode {NoiseMap, Mesh, FalloffMap};
	public DrawMode drawMode;

	public MeshSettings meshSettings;
	public HeightMapSettings heightMapSettings;
	public TextureData textureData;

	public Material terrainMaterial;

	//Water
	[SerializeField]
    private Transform water;
    public Transform Water { get { return water; } }

	public Transform meshTransform;
	//

	[Range(0,MeshSettings.numSupportedLODs-1)]
	public int editorPreviewLOD;

	public bool autoUpdate;

	public void DrawMapInEditor() {
		//textureData.ApplyToMaterial (terrainMaterial);
		//textureData.UpdateMeshHeights (terrainMaterial, heightMapSettings.minHeight, heightMapSettings.maxHeight);
		HeightMap heightMap = HeightMapGenerator.GenerateHeightMap (meshSettings.numVertsPerLine, meshSettings.numVertsPerLine, heightMapSettings, Vector2.zero);
		
		if (drawMode == DrawMode.NoiseMap) {
			Debug.Log("Drawing NoiseMap");
			DrawTexture (TextureGenerator.TextureFromHeightMap (heightMap));
		} else if (drawMode == DrawMode.Mesh) {
			DrawMesh (MeshGenerator.GenerateTerrainMesh (heightMap.values, editorPreviewLOD, meshSettings, heightMapSettings));
			DrawWater(meshSettings);
		} else if (drawMode == DrawMode.FalloffMap) {
			DrawTexture(TextureGenerator.TextureFromHeightMap(new HeightMap(FalloffGenerator.GenerateFalloffMap(meshSettings.numVertsPerLine), 0, 1)));
		}
	}

	void OnValuesUpdated() {
		if (!Application.isPlaying) {
			DrawMapInEditor ();
		}
	}

	void OnTextureValuesUpdated() {
		textureData.ApplyToMaterial (terrainMaterial);
	}

	void OnValidate() {

		if (meshSettings != null) {
			meshSettings.OnValuesUpdated -= OnValuesUpdated;
			meshSettings.OnValuesUpdated += OnValuesUpdated;
		}
		if (heightMapSettings != null) {
			heightMapSettings.OnValuesUpdated -= OnValuesUpdated;
			heightMapSettings.OnValuesUpdated += OnValuesUpdated;
		}
		if (textureData != null) {
			textureData.OnValuesUpdated -= OnTextureValuesUpdated;
			textureData.OnValuesUpdated += OnTextureValuesUpdated;
		}

	}

	public void DrawTexture(Texture2D texture) {
		textureRender.sharedMaterial.mainTexture = texture;
		textureRender.transform.localScale = new Vector3 (texture.width, 1, texture.height) / 10f;
		textureRender.gameObject.SetActive (true);
		meshFilter.gameObject.SetActive (false);
		
	}

	public void DrawMesh(MeshData meshData) {
		meshFilter.sharedMesh = meshData.CreateMesh ();
		textureRender.gameObject.SetActive(false);
		meshFilter.gameObject.SetActive(true);
	}

	public void DrawWater(MeshSettings meshSettings){
		water.parent = meshTransform;
		float waterSideLength = meshSettings.meshWorldSize;
		water.localScale = new Vector3(waterSideLength / 10f, 1, waterSideLength / 10f);
		water.localPosition = new Vector3(meshTransform.localPosition.x, 1.5f, meshTransform.localPosition.z);
	}

}
