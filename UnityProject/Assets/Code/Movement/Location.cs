using UnityEngine;
using System.Collections;
using System;
using AssemblyCSharp;

public class Location : MonoBehaviour {

	private static readonly float SNAP_THRESHOLD = 0.5f;
	public static Location activeLocation;
	
	public float maxDistance = 7;
	public Boolean useFavoredDirection = false;
	public DirectionType favoredDirection = DirectionType.NORTH;
	
	private DirectionType curD = DirectionType.NORTH;
	
	void Start () {		
		Vector3 cameraPosition = Camera.mainCamera.transform.position;
		bool isActive = (cameraPosition - transform.position).magnitude <= SNAP_THRESHOLD;
		if (isActive) {
			moveHere();
		}
	}
	
	void OnMouseUpAsButton() {		
		float distance = (Camera.mainCamera.transform.position - transform.position).magnitude;
		if (distance <= maxDistance)
			moveHere();
	}
	
	void OnDrawGizmos() {
		Gizmos.DrawIcon(transform.position, "LocationGizmo.png");
	}
	
	public void moveHere() {
		Direction favored = null;
		if (useFavoredDirection)
			favored = getAt(favoredDirection);
		
		Direction[] closest = getNearestDirections(Camera.mainCamera.transform.rotation);
		Direction turnTo = closest[0];
		
		// If favored is activated & the favored direction is present,
		// decide whether we should select the favored direction instead
		if (favored != null && favoredDirection != turnTo.direction) {
			int indexOfFavored = -1;
			for (int i = 0; i < closest.Length; i++) {
				if (closest[i].Equals(favored)) {
					indexOfFavored = i;
					break;
				}
			}
			
			if (indexOfFavored != -1 && indexOfFavored <= Math.Ceiling(((double) closest.Length) / 2.0))
				turnTo = favored;
		}
		
		curD = turnTo.direction;
		CameraAction action = new CameraAction(this, transform.position, turnTo.rotation);
		CameraController.instance.addAction(action);
	}
	
	public void activate() {
		if (activeLocation != null)
			activeLocation.deactivate();
		
		activeLocation = this;
		this.collider.enabled = false;
	}
	
	public void deactivate() {
		this.collider.enabled = true;
	}
	
	public void turnLeft() {
		DirectionType d = DirectionUtil.getLeft(curD);
		Direction dir = null;
		
		int i = 0;
		while (dir == null && d != curD && i < 4) {
			dir = getAt (d);
			d = DirectionUtil.getLeft(d);
			i++;
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
			dir = getAt (d);
			d = DirectionUtil.getRight(d);
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
	
	private Direction[] getNearestDirections(Quaternion rot) {
		Component[] dirs = GetComponents(typeof(Direction));
		
		Direction[] ret = new Direction[dirs.Length];
		
		for (int i = 0; i < dirs.Length; i++) {
			Direction cur = (Direction) dirs[i];
			float ang = angleDiff(cur.rotation, rot);
			
			int j = 0;
			while (ret[j] != null && ang >= angleDiff(ret[j].rotation, rot)) {
				j++;
			}
			
			while (cur != null) {
				Direction temp = ret[j];
				ret[j] = cur;
				cur = temp;
				j++;
			}
		}
		
		return ret;
	}
	
	private float angleDiff(Quaternion q1, Quaternion q2) {
		return Quaternion.Angle(q1, q2);
	}
}
