using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using AssemblyCSharp;

public class CameraController : MonoBehaviour {
	
	public static CameraController instance;
	
	private CameraMovement movementControl;
	
	CameraAction next = null;
	CameraAction cur = null;
	
	void Start () {
		instance = this;
		try {
			transform.position = GameState.getInstance().getCameraPosition();
			transform.rotation = GameState.getInstance().getCameraRotation();
		} catch (Exception e) {
			GameState.getInstance().setCameraPosition(transform.position);
			GameState.getInstance().setCameraRotation(transform.rotation);
		}
		movementControl = new SineMovement(transform.position, transform.rotation);
	}
	
	void Update () {
		if (cur == null && next != null) {
			cur = next;
			next = null;
			cur.getLocation().activate();
			
			Vector3 goalPos = cur.hasPosition() ? cur.getPosition() : transform.position;
			Quaternion goalRot = cur.hasRotation() ? cur.getRotation() : transform.rotation;
			movementControl.reset(goalPos, goalRot);
		}
		
		if (cur != null) {
			float t = Time.time;
			movementControl.update(transform, t);
			
			if (movementControl.isDone(t)){				
				if (cur.hasPosition())
					transform.position = cur.getPosition();
				if (cur.hasRotation())
					transform.rotation = cur.getRotation();
				
				cur = null;
				
				GameState.getInstance().setCameraPosition(transform.position);
				GameState.getInstance().setCameraRotation(transform.rotation);
			}
		}
		
//		if ((!isMoving || movementControl.percentDone(Time.time) > MOVE_THRESHOLD)
//			&& Input.GetMouseButtonDown(0) && room.clicked(Input.mousePosition)) {
//			
//			movementControl = new SineMovement(transform.position, room.getPosition(),
//				transform.rotation, room.getRotation());
//			isMoving = true;
//		}
//		
//		if (isMoving) {
//			if (movementControl.isDone(Time.time)) {
//				isMoving = false;
//			} else {
//				movementControl.update(transform);
//			}
//		}
	}
	
	public void addAction(CameraAction action) {
		next = action;
	}
	
	public void moveTo(Location initiator, Vector3 position) {
		addAction(new CameraAction(initiator, position, null));
	}
	
	public void turnTo(Location initiator, Quaternion rotation) {
		addAction(new CameraAction(initiator, null, rotation));
	}
}

