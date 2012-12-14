using UnityEngine;
using System.Collections;
using System;
using AssemblyCSharp;

public class CameraController : MonoBehaviour {
	
	//Constants
	private static readonly float MOVE_THRESHOLD = 0.4f;
	
	//Debug stuff
	private static readonly Boolean debug = false;
	private static readonly Rect screenRect = new Rect(10, 10, 200, 100);
	public static string debugText = "...";
	
	//Data
	private RoomData room;
	private bool isMoving = false;
	private CameraMovement movementControl;
	
	// Use this for initialization
	void Start () {
		room = GameState.getInstance().getRoom();
		transform.position = room.getPosition();
		transform.rotation = room.getRotation();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey("escape")) {
			Application.Quit();
		}
		
		if ((!isMoving || movementControl.percentDone(Time.time) > MOVE_THRESHOLD)
			&& Input.GetMouseButtonDown(0) && room.clicked(Input.mousePosition)) {
			
			movementControl = new SineMovement(transform.position, room.getPosition(),
				transform.rotation, room.getRotation());
			isMoving = true;
		}
		
		if (isMoving) {
			if (movementControl.isDone(Time.time)) {
				isMoving = false;
			} else {
				movementControl.update(transform);
			}
		}
	}
	
	void OnGUI() {
		if (GUI.Button(new Rect(Screen.width - 35, 10, 25, 20), "X")) {
			Application.Quit();
		}
		
		if (debug) {
    		GUI.Label(screenRect, debugText);
			if (room != null) {
				foreach (Transition t in room.getTransitions()) {
					GUI.Box(t.screenArea, "Move to " + t.moveTo);
				}
				
				foreach (HotPoint hp in room.getHotPoints()) {
					GUI.Box(hp.screenArea, "HotPoint");
				}
			}
		}
	}
}

