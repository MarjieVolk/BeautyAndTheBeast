using UnityEngine;
using System.Collections;
using System;
using AssemblyCSharp;

public class ClickAndDragTranslate : ClickAndDrag
{
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
	
	public float snapSpeed = 10;
	
	private Vector3 p;
	
	protected override void setVisualState(int snapToIndex) {
		Vector3 pos = snapTo[snapToIndex];
		setPosition(pos);
	}
	
	protected override void initDrag(Vector3 dragStartMousePosition) {
		p = transform.localPosition;
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
	
	protected override void doDrag(Vector3 dragStartMousePosition, Vector3 currentMousePosition) {
		float dMouseX = currentMousePosition.x - dragStartMousePosition.x;
		float dMouseY = currentMousePosition.y - dragStartMousePosition.y;
		
		float dX = (dXPerDMouseX * dMouseX) + (dXPerDMouseY * dMouseY);
		float dY = (dYPerDMouseX * dMouseX) + (dYPerDMouseY * dMouseY);
		float dZ = (dZPerDMouseX * dMouseX) + (dZPerDMouseY * dMouseY);
		
		setPosition(new Vector3(p.x + dX, p.y + dY, p.z + dZ));
	}
	
	protected override bool doSnap(int snapToIndex) {
		Vector3 direction = snapTo[snapToIndex] - transform.localPosition;
		Vector3 newPosition = (direction.normalized * snapSpeed * Time.deltaTime);
		
		if (newPosition.magnitude > direction.magnitude) {
			setPosition(snapTo[snapToIndex]);
			return true;
		} else {
			setPosition(newPosition + transform.localPosition);
			return false;
		}
	}
	
	private void setPosition(Vector3 newPosition) {
		newPosition.x = Math.Min(maxX, Math.Max(newPosition.x, minX));
		newPosition.y = Math.Min(maxY, Math.Max(newPosition.y, minY));
		newPosition.z = Math.Min(maxZ, Math.Max(newPosition.z, minZ));
		
		transform.localPosition = newPosition;
	}
}

