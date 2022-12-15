using UnityEngine;

[CreateAssetMenu()]
public class HeightMapSettings : UpdatableData {

	//Noise settings
	public NoiseSettings noiseSettings;

	//Height multiplier
	public float heightMultiplier;
	//HeightCurve for the heightMap
	public AnimationCurve heightCurve;
	//Gradient used to color the mesh considering the height of the terrain
	public Gradient gradient;

	//Minimum value of height for the mesh
	public float minHeight{
		get{
			return heightMultiplier * heightCurve.Evaluate(0);
		}
	}

	//Maximum value of height for the mesh
	public float maxHeight{
		get{
			return heightMultiplier * heightCurve.Evaluate(1);
		}
	}

	#if UNITY_EDITOR

	/***
	Validates the heightMap settings.
	***/
	protected override void OnValidate() {
		noiseSettings.ValidateValues();
		base.OnValidate ();
	}

	#endif

}
