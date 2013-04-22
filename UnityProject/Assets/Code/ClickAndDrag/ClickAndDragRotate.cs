using System;
using AssemblyCSharp;
using UnityEngine;

public class ClickAndDragRotate: ClickAndDrag
{
	public static readonly String PARAM_OLD_R = "cadr.oldR";
	public static readonly String PARAM_DESIRED_NEW_R = "cadr.desiredNewR";
	public static readonly String PARAM_NEW_R = "cadr.newR";
	public static readonly String PARAM_D_ROTATION_X = "cadr.dR.x";
	public static readonly String PARAM_D_ROTATION_Y = "cadr.dR.y";
	public static readonly String PARAM_D_ROTATION_Z = "cadr.dR.z";
	
	private static readonly float SNAP_DEG_PER_SEC = 200;
	
	public AudioClip rotateSound = null;
	
	public Quaternion[] snapTo;
	
	public bool clampX;
	public float minX;
	public float maxX;
	
	public bool clampY;
	public float minY;
	public float maxY;
	
	public bool clampZ;
	public float minZ;
	public float maxZ;
	
	private Vector3 dDragRotation;
	private Quaternion snapStartRotation;
	private float snapStartTime = 0;
	private float snapDuration;
	
	private AudioSource sound = null;
	
	void Start() {
		if (rotateSound != null) {
			sound = (AudioSource) this.gameObject.AddComponent(typeof(AudioSource));
			sound.clip = rotateSound;
			sound.loop = true;
		}
	}
	
	protected override void setVisualState(int snapToIndex) {
		setRotation(snapTo[snapToIndex]);
	}
	
	protected override void initDrag(Vector3 dragStartMousePosition) {
		Quaternion calculatedR = getDirection(dragStartMousePosition);
		Quaternion observedR = transform.localRotation;
		dDragRotation = observedR.eulerAngles - calculatedR.eulerAngles;
		
		playSound();
	}
	
	protected override int initSnap() {
		int index = 0;
		float minAngle = Quaternion.Angle(snapTo[0], transform.localRotation);
		
		for (int i = 1; i < snapTo.Length; i++) {
			float angle = Quaternion.Angle(snapTo[i], transform.localRotation);
			if (angle < minAngle) {
				minAngle = angle;
				index = i;
			}
			
		}
		
		snapStartRotation = transform.localRotation;
		snapStartTime = Time.time;
		snapDuration = Quaternion.Angle(snapStartRotation, snapTo[index]) / SNAP_DEG_PER_SEC;
		
		return index;
	}
	
	protected override void doDrag(DragEvent toPopulate, Vector3 dragStartMousePosition, Vector3 currentMousePosition) {
		Quaternion oldR = transform.localRotation;
		Quaternion desiredNewR = Quaternion.Euler(getDirection(currentMousePosition).eulerAngles + dDragRotation);
		setRotation(desiredNewR);
		Quaternion newR = transform.localRotation;
		
		toPopulate.putParam(PARAM_OLD_R, oldR);
		toPopulate.putParam(PARAM_NEW_R, newR);
		toPopulate.putParam(PARAM_DESIRED_NEW_R, desiredNewR);
//		toPopulate.putParam(PARAM_D_ROTATION_X, dX);
//		toPopulate.putParam(PARAM_D_ROTATION_Y, dY);
//		toPopulate.putParam(PARAM_D_ROTATION_Z, dZ);
	}
	
	protected override bool doSnap(DragEvent toPopulate, int snapToIndex) {
		float percent = (Time.time - snapStartTime) / snapDuration;
		if (percent >= 1) {
			if (sound != null)
				sound.Stop();
			setVisualState(snapToIndex);
			return true;
		}
		
		setRotation(Quaternion.Slerp(snapStartRotation, snapTo[snapToIndex], percent));
		return false;
	}
	
	protected override int getDefaultVal() {
		return 0;
	}
	
	protected void playSound() {
		if (sound != null)
			sound.Play();
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
	
	protected void setRotation(Quaternion rotation) {
		Quaternion clampedRotation = getClampedRotation(rotation);
		transform.localRotation = clampedRotation;
	}
	
	private Quaternion getDirection(Vector3 mouseP) {
		RaycastHit hit;
		Camera cam = CameraController.instance.camera;
		Physics.Raycast(cam.ScreenPointToRay(mouseP), out hit);
		Vector3 hitPoint = transform.parent == null ? hit.point : transform.parent.InverseTransformPoint(hit.point);
		Vector3 direction = hitPoint - transform.localPosition;
		return Quaternion.LookRotation(direction);
	}
}
