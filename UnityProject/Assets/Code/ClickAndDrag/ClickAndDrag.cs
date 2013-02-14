using UnityEngine;
using System.Collections;
using System;
using AssemblyCSharp;

public abstract class ClickAndDrag : MonoBehaviour {

	private static readonly int NONE = 0;
	private static readonly int DRAG = 1;
	private static readonly int SNAP = 2;
	
	public String gameStateKey;
	
	private int state = NONE;
	private Vector3 dragStartMousePos;
	private int snapToIndex;
	
	protected abstract void setVisualState(int snapToIndex);
	protected abstract void initDrag(Vector3 dragStartMousePosition);
	protected abstract int initSnap();
	protected abstract void doDrag(Vector3 dragStartMousePosition, Vector3 currentMousePosition);
	protected abstract bool doSnap(int snapToIndex);
	
	void Start () {
		if (gameStateKey != null && GameState.getInstance().has(gameStateKey)) {
			snapToIndex = (int) GameState.getInstance().get(gameStateKey);
			setVisualState(snapToIndex);
		}
	}
	
	void Update () {
		if (!Input.GetMouseButton(0) && state == DRAG) {
			endDrag();
		}
		
		if (state == DRAG) {
			doDrag(dragStartMousePos, Input.mousePosition);
			
		} else if (state == SNAP && doSnap(snapToIndex)) {
			state = NONE;
		}
	}
	
	void OnMouseOver() {
		// If mouse is pressed
		if (state != DRAG && Input.GetMouseButtonDown(0)) {
			state = DRAG;
			dragStartMousePos = Input.mousePosition;
			initDrag(dragStartMousePos);
		}
	}
	
	private void endDrag() {
		snapToIndex = initSnap();
		state = SNAP;
		
		if (gameStateKey != null) {
			GameState.getInstance().put(gameStateKey, snapToIndex);
		}
	}
}
