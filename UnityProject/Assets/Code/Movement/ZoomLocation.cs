using System;
using UnityEngine;

public class ZoomLocation: Location
{
	private static readonly float BUTTON_WIDTH = 50;
	private static readonly float BUTTON_HEIGHT = 30;
	private static readonly float BUTTON_MARGIN = 20;
	
	public Location parent = null;
	public Quaternion rotation;
	
	void Start() {		
		// If parent unspecified, choose closest Location as parent
		if (parent == null) {
			UnityEngine.Object[] allLocations = GameObject.FindObjectsOfType(typeof(Location));
			Location closest = null;
			float closestDistance = -1;
			
			for (int i = 0; i < allLocations.Length; i++) {
				Location cur = (Location) allLocations[i];
				if (cur is ZoomLocation)
					continue;
				
				float dist = Vector3.Distance(cur.transform.position, transform.position);
				if (closestDistance == -1 || dist < closestDistance) {
					closestDistance = dist;
					closest = cur;
				}
			}
			
			parent = closest;
		}
		
		init();
	}
	
	public override void moveHere() {
		CameraController.instance.moveTo(this, transform.position + offset, rotation);	
	}
	
	public override bool canMoveHere() {
		return parent.Equals(Location.activeLocation) || parent.canMoveHere();
	}
	
	public override void activate() {
		base.activate();
		parent.collider.enabled = false;
	}
	
	public override void deactivate() {
		base.deactivate();
		parent.collider.enabled = true;
	}
	
	void OnGUI() {
		if (activeLocation == this) {
			if (GUI.Button(new Rect((Screen.width - BUTTON_WIDTH) / 2.0f, BUTTON_MARGIN, BUTTON_WIDTH, BUTTON_HEIGHT), "Back")) {
				parent.moveHere();
			}
		}
	}
	
	void OnDrawGizmos() {
		Gizmos.DrawIcon(transform.position, "ZoomLocationGizmo.png");
	}
}

