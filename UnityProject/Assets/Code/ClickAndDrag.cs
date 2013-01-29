using UnityEngine;
using System.Collections;
using System;
using AssemblyCSharp;

public class ClickAndDrag : MonoBehaviour {
	
	private static readonly int NONE = 0;
	private static readonly int DRAG = 1;
	private static readonly int SNAP = 2;
	
	
	public String animName;
	public String gameStateKey;
	public float[] snapTo;
	public float dAnimationTimePerDMouseY;
	
	private int state = NONE;
	private Vector3 dragStartMousePos;
	private float dragStartAnimPos;
	private int snapToIndex;
	
	void Start () {
		if (gameStateKey != null && GameState.getInstance().has(gameStateKey)) {
			snapToIndex = (int) GameState.getInstance().get(gameStateKey);
			animation[animName].time = snapTo[snapToIndex];
			animation[animName].speed = 0;
			animation.Play (animName);
		}
	}
	
	void Update () {
		if (!Input.GetMouseButton(0) && state == DRAG) {
			endDrag();
		}
		
		if (state == DRAG) {
			float dY = (Input.mousePosition - dragStartMousePos).y / ((float) Screen.height);
			float dTime = dAnimationTimePerDMouseY * dY * animation[animName].length;
			animation[animName].time = Mathf.Clamp(dragStartAnimPos + dTime, 0.0f, animation[animName].length);
		
			animation[animName].speed = 0;
			animation.Play(animName);
			
		} else if (state == SNAP) {
			float t = animation[animName].time;
			
			if (animation[animName].speed == 1) {
				if (t >= snapTo[snapToIndex] || t == 0) {
					endSnap();
				}
			} else if (t <= snapTo[snapToIndex]) {
				endSnap();
			}
		}
	}
	
	void OnMouseOver() {
		// If mouse is pressed
		if (state != DRAG && Input.GetMouseButton(0)) {
			animation[animName].speed = 0;
			animation.Play(animName);
			startDrag();
		}
	}
	
	private void startDrag() {
		state = DRAG;
		dragStartMousePos = Input.mousePosition;
		dragStartAnimPos = animation[animName].time;
		
		animation[animName].speed = 0;
		animation.Play(animName);
		
		CameraController.debugText = "Start Drag";
	}
	
	private void endDrag() {
		float minDistance = float.MaxValue;
		int index = -1;
		float time = animation[animName].time;
		for (int i = 0; i < snapTo.Length; i++) {
			float distance = Math.Abs(snapTo[i] - time);
			if (distance < minDistance) {
				minDistance = distance;
				index = i;
			}
		}
			
		snapToIndex = index;
		state = SNAP;
		
		if (animation[animName].time < snapTo[snapToIndex]) {
			animation[animName].speed = 1;
		} else {
			animation[animName].speed = -1;
		}
		
		animation.Play(animName);
		
		if (gameStateKey != null) {
			GameState.getInstance().put(gameStateKey, snapToIndex);
		}
		
		CameraController.debugText = "End Drag, Start Snap";
	}
	
	private void endSnap() {
		animation[animName].time = snapTo[snapToIndex];
		animation[animName].speed = 0;
		animation.Play(animName);
		state = NONE;
		
		CameraController.debugText = "End Snap";
	}
	
	private bool contains(float[] list, float val) {
		foreach (float  f in list) {
			if (f == val)
				return true;
		}
		return false;
	}
}
