using System;
using UnityEngine;
using AssemblyCSharp;

public class RotationDragModifier: DragModifier
{
	
	public GameObject toRotate;
	
	public float dXPerDBaseX = 0;
	public float dXPerDBaseY = 0;
	public float dXPerDBaseZ = 0;
	
	public float dYPerDBaseX = 0;
	public float dYPerDBaseY = 0;
	public float dYPerDBaseZ = 0;
	
	public float dZPerDBaseX = 0;
	public float dZPerDBaseY = 0;
	public float dZPerDBaseZ = 0;
	
	private Vector3 dragStartR;
	
	public override void startDrag() {
		dragStartR = toRotate.transform.localRotation.eulerAngles;
	}
	
	public override void endDrag() {
		
	}
	
	public override void startSnap() {
		
	}
	
	public override void endSnap() {
		
	}
	
	public override void handleDragEvent(DragEvent e) {
		if (e.state == DragState.DRAG) {
			float dBaseX = (float) e.getParam(ClickAndDragRotate.PARAM_D_ROTATION_X);
			float dBaseY = (float) e.getParam(ClickAndDragRotate.PARAM_D_ROTATION_Y);
			float dBaseZ = (float) e.getParam(ClickAndDragRotate.PARAM_D_ROTATION_Z);
			
			float dX = (dXPerDBaseX * dBaseX) + (dXPerDBaseY * dBaseY) + (dXPerDBaseZ * dBaseZ);
			float dY = (dYPerDBaseX * dBaseX) + (dYPerDBaseY * dBaseY) + (dYPerDBaseZ * dBaseZ);
			float dZ = (dZPerDBaseX * dBaseX) + (dZPerDBaseY * dBaseY) + (dZPerDBaseZ * dBaseZ);
			
			Vector3 newR = getNewRotation(dX, dY, dZ);
			toRotate.transform.localRotation = Quaternion.Euler(newR);
		}
	}
	
	protected virtual Vector3 getNewRotation(float dX, float dY, float dZ) {
		return new Vector3(dragStartR.x + dX, dragStartR.y + dY, dragStartR.z + dZ);
	}

}

