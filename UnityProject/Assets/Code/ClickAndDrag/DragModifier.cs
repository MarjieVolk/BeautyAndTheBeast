using UnityEngine;
using System.Collections;
using System;
using AssemblyCSharp;

public abstract class DragModifier : MonoBehaviour
{
	
	public abstract void startDrag();
	public abstract void endDrag();
	public abstract void startSnap();
	public abstract void endSnap();
	public abstract void handleDragEvent(DragEvent e);
	
}