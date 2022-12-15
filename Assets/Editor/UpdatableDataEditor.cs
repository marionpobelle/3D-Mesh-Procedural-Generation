using UnityEngine;
using UnityEditor;

/***
Custom editor.
Click the 'Update' button in 'Default Heightmap' or Default Mesh' allows the users to update these settings with new values on the preview.
***/
[CustomEditor (typeof(UpdatableData), true)]
public class UpdatableDataEditor : Editor {

	public override void OnInspectorGUI () {
		base.OnInspectorGUI ();
		UpdatableData data = (UpdatableData)target;
		if (GUILayout.Button ("Update")) {
			data.NotifyOfUpdatedValues ();
			EditorUtility.SetDirty(target);
		}
	}
	
}
