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
	
	private Texture2D upCursor;
	private Texture2D downCursor;
	
	protected virtual void childStart() {}
	
	protected abstract void setVisualState(int snapToIndex);
	protected abstract void initDrag(Vector3 dragStartMousePosition);
	protected abstract int initSnap();
	protected abstract void doDrag(DragEvent toPopulate, Vector3 dragStartMousePosition, Vector3 currentMousePosition);
	protected abstract bool doSnap(DragEvent toPopulate, int snapToIndex);
	protected abstract int getDefaultVal();
	
	void Start () {
		upCursor = Resources.Load("Cursors/Drag") as Texture2D;
		downCursor = Resources.Load("Cursors/Drag Down") as Texture2D;
		
		if (gameStateKey != null && GameState.getInstance().has(gameStateKey)) {
			snapToIndex = (int) GameState.getInstance().get(gameStateKey);
		} else {
			snapToIndex = getDefaultVal();
		}
		
		setVisualState(snapToIndex);
		if (gameStateKey != null && !GameState.getInstance().has(gameStateKey)) {
			GameState.getInstance().put(gameStateKey, snapToIndex);
		}
		
		childStart();
	}
	
	void Update () {		
		if (!Input.GetMouseButton(0) && state == DragState.DRAG) {
			endDrag();
			CursorManager.giveUpCursorFocus(this);
		}
		
		if (state == DragState.DRAG) {
			CursorManager.takeCursorFocus(this, downCursor, Vector2.zero);
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
		if (!canStartDrag()) {
			CursorManager.giveUpCursorFocus(this);
			return;
		}
		
		if (Input.GetMouseButton(0)) {
			CursorManager.takeCursorFocus(this, downCursor, Vector2.zero);
		} else {
			CursorManager.takeCursorFocus(this, upCursor, Vector2.zero);
		}
		
		// If mouse is pressed
		if (state != DragState.DRAG && Input.GetMouseButtonDown(0)) {
			state = DragState.DRAG;
			dragStartMousePos = Input.mousePosition;
			
			initDrag(dragStartMousePos);
			
			foreach (DragModifier m in modifiers())
				m.startDrag();
		}
	}
	
	void OnMouseExit() {
		if (state != DragState.DRAG)
			CursorManager.giveUpCursorFocus(this);
	}
	
	private bool canStartDrag() {
		float distance = (Camera.mainCamera.transform.position - transform.position).magnitude;
		return distance <= maxDistance && distance >= minDistance;
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
