using UnityEngine;
using UnityEditor;

/***
Custom editor.
Allows to get a preview of the map without entering PlayMode.
Click the 'Generate' button allows the user to get a preview with new settings.
***/
[CustomEditor (typeof (MapPreview))]
public class MapPreviewEditor : Editor {

	public override void OnInspectorGUI() {
		MapPreview mapPreview = (MapPreview)target;
		if (DrawDefaultInspector ()) {
			if (mapPreview.autoUpdate) {
				mapPreview.NullifyChildren();
				mapPreview.DrawMapInEditor ();
			}
		}
		if (GUILayout.Button ("Generate")) {
			mapPreview.NullifyChildren();
			mapPreview.DrawMapInEditor ();
		}
	}
}
