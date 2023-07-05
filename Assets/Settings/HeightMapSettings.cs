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
		get;
		protected set;
	} = 0f;

	//Maximum value of height for the mesh
	public float maxHeight{
		get;
		protected set;
	} = 1f;

	#if UNITY_EDITOR

	/***
	Validates the heightMap settings.
	***/
	protected override void OnValidate() {
		noiseSettings.ValidateValues();
		base.OnValidate ();
	}

	public void UpdateBounds()
	{
		minHeight = heightMultiplier * heightCurve.Evaluate(0);
		maxHeight = heightMultiplier * heightCurve.Evaluate(1);
	}

	#endif

}
