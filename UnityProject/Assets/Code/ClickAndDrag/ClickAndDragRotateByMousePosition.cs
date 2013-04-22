using System;
using UnityEngine;
using AssemblyCSharp;

public class ClickAndDragRotateByMousePosition : ClickAndDragRotate
{	
	public float dXPerDMouseX = 0;
	public float dXPerDMouseY = 0;
	public float dYPerDMouseX = 0;
	public float dYPerDMouseY = 0;
	public float dZPerDMouseX = 0;
	public float dZPerDMouseY = 0;
	
	private Vector3 dragStartRotation;
	
	protected override void initDrag(Vector3 dragStartMousePosition) {
		dragStartRotation = transform.localRotation.eulerAngles;
		playSound();
	}
	
	protected override void doDrag(DragEvent toPopulate, Vector3 dragStartMousePosition, Vector3 currentMousePosition) {
		float dMouseX = currentMousePosition.x - dragStartMousePosition.x;
		float dMouseY = currentMousePosition.y - dragStartMousePosition.y;
		
		float dX = (dXPerDMouseX * dMouseX) + (dXPerDMouseY * dMouseY);
		float dY = (dYPerDMouseX * dMouseX) + (dYPerDMouseY * dMouseY);
		float dZ = (dZPerDMouseX * dMouseX) + (dZPerDMouseY * dMouseY);
		
		Quaternion oldR = transform.localRotation;
		Vector3 newRotation = getNewRotation(dX, dY, dZ);
		Quaternion desiredNewR = Quaternion.Euler(newRotation);
		
		setRotation(desiredNewR);
		Quaternion newR = transform.localRotation;
		
		toPopulate.putParam(PARAM_OLD_R, oldR);
		toPopulate.putParam(PARAM_NEW_R, newR);
		toPopulate.putParam(PARAM_DESIRED_NEW_R, desiredNewR);
		toPopulate.putParam(PARAM_D_ROTATION_X, dX);
		toPopulate.putParam(PARAM_D_ROTATION_Y, dY);
		toPopulate.putParam(PARAM_D_ROTATION_Z, dZ);
	}
	
	protected Vector3 getNewRotation(float dX, float dY, float dZ) {
		Vector3 r = transform.localRotation.eulerAngles;
		return new Vector3(r.x + dX, r.y + dY, r.z + dZ);
	}
}

