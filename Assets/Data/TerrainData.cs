using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TerrainData : UpdatableData
{
    //Scales x/y/z
    public float uniformScale = 2.5f;
    //Scales on the Y axis
    public float meshHeightMultiplier;
	public AnimationCurve meshHeightCurve;
}
