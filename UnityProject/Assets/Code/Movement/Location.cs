using UnityEngine;
using System.Collections;
using System;
using AssemblyCSharp;

public class Location : MonoBehaviour {

	private static readonly float SNAP_THRESHOLD = 0.5f;
	public static Location activeLocation;
	
	public float maxDistance = 7;
	
	private DirectionType curD = DirectionType.NORTH;
	
	// Use this for initialization
	void Start () {		
		Vector3 cameraPosition = Camera.mainCamera.transform.position;
		bool isActive = (cameraPosition - transform.position).magnitude <= SNAP_THRESHOLD;
		if (isActive) {
//			activate();
//			Camera.mainCamera.transform.position = transform.position;
//			Direction dir = getNearestDirection(Camera.mainCamera.transform.rotation);
//			Camera.mainCamera.transform.rotation = dir.rotation;
//			GameState.getInstance().setCameraPosition(transform.position);
//			GameState.getInstance().setCameraRotation(dir.rotation);
//			curD = dir.direction;
			moveHere();
		}
	}
	
	void OnMouseOver() {
		if (!Input.GetMouseButtonDown(0))
			return;
		
		float distance = (Camera.mainCamera.transform.position - transform.position).magnitude;
		if (distance <= maxDistance)
			moveHere();
	}
	
	private void moveHere() {
		// TODO: be more smart about picking which direction the camera should face
		Direction turnTo = getNearestDirection(Camera.mainCamera.transform.rotation);
		curD = turnTo.direction;
		CameraAction action = new CameraAction(this, transform.position, turnTo.rotation);
		CameraController.instance.addAction(action);
	}
	
	public void activate() {
		activeLocation = this;
		GeneralRoomFeaturesScript.debugText = this.gameObject.name;
	}
	
	public void turnLeft() {
		DirectionType d = DirectionUtil.getLeft(curD);
		Direction dir = null;
		
		while (dir == null && d != curD) {
			d = DirectionUtil.getLeft(d);
			dir = getAt (d);
		}
		
		if (dir != null) {
			curD = dir.direction;
			CameraController.instance.turnTo(this, dir.rotation);
		}
	}
	
	public void turnRight() {
		DirectionType d = DirectionUtil.getRight(curD);
		Direction dir = null;
		
		int i = 0;
		while (dir == null && d != curD && i < 4) {
			d = DirectionUtil.getRight(d);
			dir = getAt (d);
			i++;
		}
		
		if (dir != null) {
			curD = dir.direction;
			CameraController.instance.turnTo(this, dir.rotation);
		}
	}
	
	private Direction getAt(DirectionType type) {
		Component[] dirs = GetComponents(typeof(Direction));
		foreach (Component c in dirs) {
			Direction d = (Direction) c;
			if (d.direction == type) {
				return d;
			}
		}
		
		return null;
	}
	
	private Direction getNearestDirection(Quaternion rot) {
		Component[] dirs = GetComponents(typeof(Direction));
		
		Direction min = (Direction) dirs[0];
		float minAngle = angleDiff(min.rotation, rot);
		
		for (int i = 1; i < dirs.Length; i++) {
			Direction cur = (Direction) dirs[i];
			float ang = angleDiff(cur.rotation, rot);
			if (ang < minAngle) {
				minAngle = ang;
				min = cur;
			}
		}
		
		return min;
	}
	
	private float angleDiff(Quaternion q1, Quaternion q2) {
		return Quaternion.Angle(q1, q2);
	}
}
