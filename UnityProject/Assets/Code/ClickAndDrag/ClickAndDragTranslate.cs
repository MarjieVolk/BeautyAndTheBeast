using UnityEngine;
using System.Collections;
using System;
using AssemblyCSharp;

public class ClickAndDragTranslate : ClickAndDrag
{
	public static readonly String PARAM_VELOCITY = "cadt.velocity";
	public static readonly String PARAM_DRAG_STRENGTH = "cadt.dragStrength";
	
	public Vector3[] snapTo;
	
	public float dXPerDMouseX = 0;
	public float dXPerDMouseY = 0;
	public float dYPerDMouseX = 0;
	public float dYPerDMouseY = 0;
	public float dZPerDMouseX = 0;
	public float dZPerDMouseY = 0;
	
	public float minX = 0;
	public float maxX = 0;
	public float minY = 0;
	public float maxY = 0;
	public float minZ = 0;
	public float maxZ = 0;
	
	private Vector3 p;
	private Vector3 v;
	
	private float a = 1.0f;
	private float speed = 0;
	
	protected override void setVisualState(int snapToIndex) {
		Vector3 pos = snapTo[snapToIndex];
		setPosition(pos);
	}
	
	protected override void initDrag(Vector3 dragStartMousePosition) {
		p = transform.localPosition;
		v = new Vector3(0, 0, 0);
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
		
		speed = v.magnitude;
		
		return index;
	}
	
	protected override void doDrag(DragEvent toPopulate, Vector3 dragStartMousePosition, Vector3 currentMousePosition) {
		float dMouseX = currentMousePosition.x - dragStartMousePosition.x;
		float dMouseY = currentMousePosition.y - dragStartMousePosition.y;
		
		float dX = (dXPerDMouseX * dMouseX) + (dXPerDMouseY * dMouseY);
		float dY = (dYPerDMouseX * dMouseX) + (dYPerDMouseY * dMouseY);
		float dZ = (dZPerDMouseX * dMouseX) + (dZPerDMouseY * dMouseY);
		
		Vector3 oldP = transform.localPosition;
		Vector3 desiredNewP = new Vector3(p.x + dX, p.y + dY, p.z + dZ);
		setPosition(desiredNewP);
		Vector3 newP = transform.localPosition;
		
		//Update velocity
		v = (newP - oldP) / Time.deltaTime;
		
		//Populate event
		toPopulate.putParam(PARAM_VELOCITY, v);
		toPopulate.putParam(PARAM_DRAG_STRENGTH, (desiredNewP - oldP) / Time.deltaTime);
	}
	
	protected override bool doSnap(DragEvent toPopulate, int snapToIndex) {
		Vector3 direction = snapTo[snapToIndex] - transform.localPosition;
		v = direction.normalized * speed;
		speed += a * Time.deltaTime;
		
		//Populate event
		toPopulate.putParam(PARAM_VELOCITY, v);
		
		if (v.magnitude * Time.deltaTime > direction.magnitude) {
			setPosition(snapTo[snapToIndex]);
			return true;
		} else {
			setPosition((v * Time.deltaTime) + transform.localPosition);
			return false;
		}
	}
	
	private void setPosition(Vector3 newPosition) {
		Vector3 p = new Vector3(Mathf.Clamp(newPosition.x, minX, maxX),
			Mathf.Clamp(newPosition.y, minY, maxY), Mathf.Clamp(newPosition.z, minZ, maxZ));
		
		transform.localPosition = p;
	}
	
	protected override int getDefaultVal() {
		return 0;
	}
}

