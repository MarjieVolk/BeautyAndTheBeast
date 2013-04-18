using System;
using AssemblyCSharp;
using UnityEngine;

public class ClickAndDragRotate: ClickAndDrag
{
	public static readonly String PARAM_OLD_R = "cadr.oldR";
	public static readonly String PARAM_DESIRED_NEW_R = "cadr.desiredNewR";
	public static readonly String PARAM_NEW_R = "cadr.newR";
	
	private static readonly float SNAP_DEG_PER_SEC = 30;
	
	public Quaternion[] snapTo;
	
	public float dXPerDMouseX = 0;
	public float dXPerDMouseY = 0;
	public float dYPerDMouseX = 0;
	public float dYPerDMouseY = 0;
	public float dZPerDMouseX = 0;
	public float dZPerDMouseY = 0;
	
	public bool clampX;
	public float minX;
	public float maxX;
	
	public bool clampY;
	public float minY;
	public float maxY;
	
	public bool clampZ;
	public float minZ;
	public float maxZ;
	
	private Vector3 dragStartRotation;
	
	private Quaternion snapStartRotation;
	private float snapStartTime;
	private float snapDuration;
	
	protected override void setVisualState(int snapToIndex) {
		setRotation(snapTo[snapToIndex]);
	}
	
	protected override void initDrag(Vector3 dragStartMousePosition) {
		dragStartRotation = transform.rotation.eulerAngles;
	}
	
	protected override int initSnap() {
		int index = 0;
		float minAngle = Quaternion.Angle(snapTo[0], transform.rotation);
		
		for (int i = 1; i < snapTo.Length; i++) {
			float angle = Quaternion.Angle(snapTo[i], transform.rotation);
			if (angle < minAngle) {
				minAngle = angle;
				index = i;
			}
			
		}
		
		snapStartRotation = transform.rotation;
		snapStartTime = Time.time;
		snapDuration = Quaternion.Angle(snapStartRotation, snapTo[index]) / SNAP_DEG_PER_SEC;
		
		return index;
	}
	
	protected override void doDrag(DragEvent toPopulate, Vector3 dragStartMousePosition, Vector3 currentMousePosition) {
		float dMouseX = currentMousePosition.x - dragStartMousePosition.x;
		float dMouseY = currentMousePosition.y - dragStartMousePosition.y;
		
		float dX = (dXPerDMouseX * dMouseX) + (dXPerDMouseY * dMouseY);
		float dY = (dYPerDMouseX * dMouseX) + (dYPerDMouseY * dMouseY);
		float dZ = (dZPerDMouseX * dMouseX) + (dZPerDMouseY * dMouseY);
		
		Quaternion oldR = transform.localRotation;
		Vector3 newRotation = new Vector3(dragStartRotation.x + dX, dragStartRotation.y + dY, dragStartRotation.z + dZ);
		Quaternion desiredNewR = Quaternion.Euler(newRotation);
		setRotation(desiredNewR);
		Quaternion newR = transform.localRotation;
		
		toPopulate.putParam(PARAM_OLD_R, oldR);
		toPopulate.putParam(PARAM_NEW_R, newR);
		toPopulate.putParam(PARAM_DESIRED_NEW_R, desiredNewR);
	}
	
	protected override bool doSnap(DragEvent toPopulate, int snapToIndex) {
		float percent = (Time.time - snapStartTime) / snapDuration;
		if (percent >= 1) {
			setVisualState(snapToIndex);
			return true;
		}
		
		Quaternion.Slerp(snapStartRotation, snapTo[snapToIndex], percent);
		return false;
	}
	
	protected override int getDefaultVal() {
		return 0;
	}
	
	public Quaternion getClampedRotation(Quaternion rotation) {
		Vector3 euler = rotation.eulerAngles;
		
		if (clampX)
			euler.x = getClampedAngle(euler.x, minX, maxX);
		
		if (clampY)
			euler.y = getClampedAngle(euler.y, minY, maxY);
		
		if (clampZ)
			euler.z = getClampedAngle(euler.z, minZ, maxZ);
		
		return Quaternion.Euler(euler);
	}
	
	private float getClampedAngle(float angle, float min, float max) {
		angle = angle % 360;
		min = min % 360;
		max = max % 360;
		
		// If angle is out of range...
		if ((min > max && angle < min && angle > max) ||
			(min <= max && (angle < min || angle > max))) {
			
			// Angle btw 'angle' and 'min'
			float angleMin = Mathf.Abs(angle - min);
			angleMin = Math.Min(angleMin, 360 - angleMin);
			
			// Angle btw 'angle' and 'max'
			float angleMax = Mathf.Abs(angle - max);
			angleMax = Math.Min(angleMax, 360 - angleMax);
			
			// Return either max or min - whichever is closest to 'angle'
			return angleMin < angleMax ? min : max;
		}
		
		return angle;
	}
	
	private void setRotation(Quaternion rotation) {
		Quaternion clampedRotation = getClampedRotation(rotation);
		transform.rotation = clampedRotation;
	}
}

