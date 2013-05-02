using UnityEngine;
using System.Collections;

public class PrintHoverItem : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		Camera cam = CameraController.instance.camera;
		Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit);
		
		if (hit.collider == null) {
			GeneralRoomFeaturesScript.debugText = "";
		} else {
			GameObject hover = hit.collider.gameObject;
			GeneralRoomFeaturesScript.debugText = "" + hover;
		}
	}
}
