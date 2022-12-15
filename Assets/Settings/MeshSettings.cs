using UnityEngine;

[CreateAssetMenu()]
public class MeshSettings : UpdatableData {
	
	//Amount of supported LODs
	public const int numSupportedLODs = 5;
	//Amount of supported chunk sizes
	public const int numSupportedChunkSizes = 9;
	//Amount of supported chunk sizes when using flatshading
	public const int numSupportedFlatshadedChunkSizes = 3;
	//Supported chunk sizes
	public static readonly int[] supportedChunkSizes = {48,72,96,120,144,168,192,216,240};
	
	//Mesh scale
	public float meshScale = 2.5f;
	//Boolean that indicates if we're using flatshading or not
	public bool useFlatShading;
	
	//Index for the chunk size
	[Range(0,numSupportedChunkSizes-1)]
	public int chunkSizeIndex;
	//Index for the chunk size when using flatshading
	[Range(0,numSupportedFlatshadedChunkSizes-1)]
	public int flatshadedChunkSizeIndex;

	//Height value of the water planes
	public float waterHeight = 1.5f;

	//Number of vertices per line of mesh rendered at LOD = 0. Includes the 2 extra vertices that are excluded from final mesh, but used for calculating normals
	public int numVertsPerLine {
		get {
			return supportedChunkSizes [(useFlatShading) ? flatshadedChunkSizeIndex : chunkSizeIndex] + 5;
		}
	}

	//Size of the mesh in the world
	public float meshWorldSize {
		get {
			return (numVertsPerLine - 3) * meshScale;
		}
	}

}
