using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class CameraController : MonoBehaviour {
	
	public RoomData room = new TestRoom();
	
	private static readonly float duration = 0.4f;
	
	private bool isMoving = false;
	private float startTime;
	
	private Vector3 startLoc;
	private Vector3 endLoc;
	private Quaternion startRot;
	private Quaternion endRot;
	
	// Use this for initialization
	void Start () {
		endLoc = room.getPosition();
		endRot = room.getRotation();
		transform.position = endLoc;
		transform.rotation = endRot;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isMoving && Input.GetMouseButtonDown(0) && room.clicked(Input.mousePosition)) {
			isMoving = true;
			startTime = Time.time;
			startLoc = endLoc;
			endLoc = room.getPosition();
			startRot = endRot;
			endRot = room.getRotation();
		}
		
		if (isMoving) {
			if ((Time.time - startTime) > duration) {
				isMoving = false;
			} else {
				float percent = (Time.time - startTime) / duration;
				transform.position = Vector3.Lerp(startLoc, endLoc, percent);
				transform.rotation = Quaternion.Lerp(startRot, endRot, percent);
			}
		}
	}
}
