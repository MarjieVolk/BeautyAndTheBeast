using System;
using UnityEngine;

public class ZoomLocation: MonoBehaviour
{
	private static readonly float BUTTON_WIDTH = 50;
	private static readonly float BUTTON_HEIGHT = 30;
	private static readonly float BUTTON_MARGIN = 20;
	
	public static ZoomLocation activeLocation = null;
	
	public Location parent = null;
	public Quaternion rotation;
	
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
		if (parent.Equals(Location.activeLocation) || parent.canMoveHere()) {
			CameraController.instance.moveTo(null, transform.position, rotation);
			activeLocation = this;
			this.collider.enabled = false;
			
			Location.activeLocation.deactivate();
			Location.activeLocation = null;
		}
	}
	
	void OnGUI() {
		if (activeLocation == this) {
			if (GUI.Button(new Rect((Screen.width - BUTTON_WIDTH) / 2.0f, BUTTON_MARGIN, BUTTON_WIDTH, BUTTON_HEIGHT), "Back")) {
				parent.moveHere();
				activeLocation = null;
				this.collider.enabled = true;
			}
		}
	}
	
	void OnDrawGizmos() {
		Gizmos.DrawIcon(transform.position, "ZoomLocationGizmo.png");
	}
}

