using UnityEngine;

public class HideOnPlay : MonoBehaviour {

	/***
	Hide preview Mesh in PlayMode.
	***/
	void Start () {
		gameObject.SetActive (false);
	}
	
}
