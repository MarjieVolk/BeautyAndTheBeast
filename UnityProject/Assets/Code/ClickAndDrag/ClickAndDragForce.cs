using UnityEngine;
using System.Collections;
using System;
using AssemblyCSharp;

[RequireComponent(typeof(ConstantForce))]
public class ClickAndDragForce: ClickAndDrag
{
	/*----------- NOTE: This isn't working yet -----------*/
	
	public static readonly String PARAM_FORCE = "cadf.force";
	
	public Vector3[] snapTo;
	
	public float fXPerDMouseX = 0;
	public float fXPerDMouseY = 0;
	public float fYPerDMouseX = 0;
	public float fYPerDMouseY = 0;
	public float fZPerDMouseX = 0;
	public float fZPerDMouseY = 0;
	
	protected override void setVisualState(int snapToIndex) {
		Vector3 pos = snapTo[snapToIndex];
		transform.localPosition = pos;
	}
	
	protected override void initDrag(Vector3 dragStartMousePosition) {
		
	}
	
	protected override int initSnap() {
		float minDistance = float.MaxValue;
		int index = -1;
		
		for (int i = 0; i < snapTo.Length; i++) {
			float distance = (transform.localPosition - snapTo[i]).magnitude;
			if (distance < minDistance) {
				minDistance = distance;
				index = i;
			}
		}
		
		return index;
	}
	
	protected override void doDrag(DragEvent toPopulate, Vector3 dragStartMousePosition,
		Vector3 currentMousePosition) {
		
		float dMouseX = currentMousePosition.x - dragStartMousePosition.x;
		float dMouseY = currentMousePosition.y - dragStartMousePosition.y;
		
		float fX = (fXPerDMouseX * dMouseX) + (fXPerDMouseY * dMouseY);
		float fY = (fYPerDMouseX * dMouseX) + (fYPerDMouseY * dMouseY);
		float fZ = (fZPerDMouseX * dMouseX) + (fZPerDMouseY * dMouseY);
		
		Vector3 force = new Vector3(fX, fY, fZ);
		this.constantForce.force = force;
		
		GeneralRoomFeaturesScript.debugText = "DRAG: " + force;
		
		//Populate event
		toPopulate.putParam(PARAM_FORCE, force);
	}
	
	protected override bool doSnap(DragEvent toPopulate, int snapToIndex) {
		if ((snapTo[snapToIndex] - transform.localPosition).magnitude <= 0.001) {
			constantForce.force = Vector3.zero;
			transform.position = snapTo[snapToIndex];
			
			GeneralRoomFeaturesScript.debugText = "DONE";
			return true;
		}
		
		Vector3 force = new Vector3(0, 0, 0);
		
		foreach (Vector3 v in snapTo) {
			Vector3 distance = v - transform.localPosition;
			Vector3 fCurrent = distance.normalized / (float) Math.Pow(distance.magnitude, 2);
			force = force + fCurrent;
		}
		
		GeneralRoomFeaturesScript.debugText = "SNAP: " + force;
		
		this.constantForce.force = force;
		return false;
	}
	
	protected override int getDefaultVal() {
		return 0;
	}
}

