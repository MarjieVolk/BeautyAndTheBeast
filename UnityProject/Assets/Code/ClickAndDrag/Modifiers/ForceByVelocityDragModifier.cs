using UnityEngine;
using System;
using AssemblyCSharp;

public class ForceByVelocityDragModifier : DragModifier
{
	
	public GameObject child;
	public Vector3 maxVelocity;
	
	private bool isDrag = false;
	
	public override void startDrag() {
		isDrag = true;
	}
	
	public override void endDrag() {
		child.constantForce.force = new Vector3(0, 0, 0);
		isDrag = false;
	}
	
	public override void startSnap() {
		
	}
		
	public override void endSnap() {
		
	}
		
	public override void handleDragEvent(DragEvent e) {
		if (e.state != DragState.DRAG || !isDrag)
			return;
			
		object o = e.getParam(ClickAndDragTranslate.PARAM_DRAG_STRENGTH);
		if (!(o is Vector3))
			throw new Exception("Expected velocity to be of type Vector3");
			
		Vector3 v = (Vector3) o;
		
		// This finds the length of the component of v which is parallel to maxVelocity
		float speed = Vector3.Dot(v, maxVelocity.normalized);
		
		Vector3 force = maxVelocity.normalized;
		force = force * speed;
		child.constantForce.force = force;
	}
}

