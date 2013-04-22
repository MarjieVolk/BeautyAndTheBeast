using UnityEngine;
using System.Collections;
using System;
using AssemblyCSharp;

public abstract class ClickAndDrag : Activatable {
	
	public static readonly float NO_MAX_DISTANCE = -1;
	
	public String gameStateKey;
	public float maxDistance = 3;
	public float minDistance = 0;
	public bool isActive = true; // Right now isActive determines only whether main drag will occur. Modifiers are unaffected.
	
	private DragState state = DragState.NONE;
	private Vector3 dragStartMousePos;
	private int snapToIndex;
	
	protected abstract void setVisualState(int snapToIndex);
	protected abstract void initDrag(Vector3 dragStartMousePosition);
	protected abstract int initSnap();
	protected abstract void doDrag(DragEvent toPopulate, Vector3 dragStartMousePosition, Vector3 currentMousePosition);
	protected abstract bool doSnap(DragEvent toPopulate, int snapToIndex);
	protected abstract int getDefaultVal();
	
	void Start () {
		if (gameStateKey != null && GameState.getInstance().has(gameStateKey)) {
			snapToIndex = (int) GameState.getInstance().get(gameStateKey);
		} else {
			snapToIndex = getDefaultVal();
		}
		
		setVisualState(snapToIndex);
		if (gameStateKey != null && !GameState.getInstance().has(gameStateKey)) {
			GameState.getInstance().put(gameStateKey, snapToIndex);
		}
	}
	
	void Update () {		
		if (!Input.GetMouseButton(0) && state == DragState.DRAG)
			endDrag();
		
		if (state == DragState.DRAG) {
			DragEvent e = new DragEvent(DragState.DRAG);
			
			doDrag(e, dragStartMousePos, Input.mousePosition);
			
			foreach (DragModifier m in modifiers())
					m.handleDragEvent(e);	
			
		} else if (state == DragState.SNAP) {
			DragEvent e = new DragEvent(DragState.SNAP);
			bool isSnapDone = doSnap(e, snapToIndex);
			foreach (DragModifier m in modifiers())
					m.handleDragEvent(e);	
			if (isSnapDone) {
				state = DragState.NONE;
				foreach (DragModifier m in modifiers())
					m.endSnap();	
			}
		}
		
		if (!isActive)
			setVisualState(snapToIndex);
	}
	
	void OnMouseOver() {
		float distance = (Camera.mainCamera.transform.position - transform.position).magnitude;
		if (distance > maxDistance || distance < minDistance)
			return;
		
		// If mouse is pressed
		if (state != DragState.DRAG && Input.GetMouseButtonDown(0)) {
			state = DragState.DRAG;
			dragStartMousePos = Input.mousePosition;
			
			initDrag(dragStartMousePos);
			
			foreach (DragModifier m in modifiers())
				m.startDrag();
		}
	}
	
	private void endDrag() {
		int newSnapToIndex = initSnap();
		if (isActive)
			snapToIndex = newSnapToIndex;
			
		state = DragState.SNAP;
		foreach (DragModifier m in modifiers()) {
			m.endDrag();
			m.startSnap();
		}
		
		if (gameStateKey != null) {
			GameState.getInstance().put(gameStateKey, snapToIndex);
		}
	}
	
	private DragModifier[] modifiers() {
		Component[] comps = this.GetComponents(typeof(DragModifier));
		DragModifier[] dms = new DragModifier[comps.Length];
		for (int i = 0; i < comps.Length; i++) {
			dms[i] = (DragModifier) comps[i];
		}
		return dms;
	}
	
	public override void setActive(bool active) {
		isActive = active;
	}
}
