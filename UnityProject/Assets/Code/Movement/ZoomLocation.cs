using System;
using UnityEngine;

public class ZoomLocation: MonoBehaviour
{
	private static readonly float BUTTON_WIDTH = 50;
	private static readonly float BUTTON_HEIGHT = 30;
	private static readonly float BUTTON_MARGIN = 20;	
	
	public Location parent = null;
	public Quaternion rotation;
	
	private bool isActive = false;
	
	void Start() {
		// If parent unspecified, choose closest Location as parent
		if (parent == null) {
			UnityEngine.Object[] allLocations = GameObject.FindObjectsOfType(typeof(Location));
			Location closest = (Location) allLocations[0];
			float closestDistance = Vector3.Distance(closest.transform.position, transform.position);
			
			for (int i = 1; i < allLocations.Length; i++) {
				Location cur = (Location) allLocations[i];
				float dist = Vector3.Distance(cur.transform.position, transform.position);
				if (dist < closestDistance) {
					closestDistance = dist;
					closest = cur;
				}
			}
			
			parent = closest;
		}
	}
	
	void OnMouseUpAsButton() {
		if (parent.Equals(Location.activeLocation)) {
			CameraController.instance.moveTo(null, transform.position, rotation);
			isActive = true;
			this.collider.enabled = false;
			Location.activeLocation = null;
		}
	}
	
	void OnGUI() {
		if (isActive) {
			if (GUI.Button(new Rect((Screen.width - BUTTON_WIDTH) / 2.0f, BUTTON_MARGIN, BUTTON_WIDTH, BUTTON_HEIGHT), "Back")) {
				parent.moveHere();
				isActive = false;
				this.collider.enabled = true;
			}
		}
	}
	
	void OnDrawGizmos() {
		Gizmos.DrawIcon(transform.position, "ZoomLocationGizmo.png");
	}
}

