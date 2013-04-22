using System;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(ClickAndDragTranslate))]
public class ClickAndDragTranslateEditor : Editor
{
	private static bool snapOpen = false;
	
	private ClickAndDragTranslate scriptTarget;
	
	private bool allowX = false;
	private bool allowY = false;
	private bool allowZ = false;
	
	private Vector3[] snapPoints;
	
	void OnEnable() {
		scriptTarget = (ClickAndDragTranslate) target;
		
		if (scriptTarget.minX == 0.0f &&
			scriptTarget.maxX == 0.0f &&
			scriptTarget.minY == 0.0f &&
			scriptTarget.maxY == 0.0f &&
			scriptTarget.minZ == 0.0f &&
			scriptTarget.maxZ == 0.0f) {
			// this has never been edited
			
			scriptTarget.minX = scriptTarget.maxX = scriptTarget.transform.localPosition.x;
			scriptTarget.minY = scriptTarget.maxY = scriptTarget.transform.localPosition.y;
			scriptTarget.minZ = scriptTarget.maxZ = scriptTarget.transform.localPosition.z;
		}
		
		if (scriptTarget.snapTo == null)
			scriptTarget.snapTo = new Vector3[] { scriptTarget.getClampedPosition(new Vector3(0, 0, 0)) };
		
		snapPoints = scriptTarget.snapTo;
		
		Vector3 p = scriptTarget.transform.localPosition;
		allowX = !(p.x == scriptTarget.minX && p.x == scriptTarget.maxX);
		allowY = !(p.y == scriptTarget.minY && p.y == scriptTarget.maxY);
		allowZ = !(p.z == scriptTarget.minZ && p.z == scriptTarget.maxZ);
	}
	
	public override void OnInspectorGUI() {
		scriptTarget.gameStateKey = EditorGUILayout.TextField("Game State Key", scriptTarget.gameStateKey);
		scriptTarget.isActive = EditorGUILayout.Toggle("Active", scriptTarget.isActive);
		scriptTarget.maxDistance = EditorGUILayout.FloatField("Max Interaction Distane", scriptTarget.maxDistance);
		scriptTarget.minDistance = EditorGUILayout.FloatField("Min Interaction Distane", scriptTarget.minDistance);
		
		snapOpen = EditorGUILayout.Foldout(snapOpen, "Snap points");
		if (snapOpen) {
			EditorGUI.indentLevel++;
			
			int newLength = EditorGUILayout.IntField("Number", snapPoints.Length);
			if (newLength != snapPoints.Length) {
				Vector3[] newSnapPoints = new Vector3[newLength];
				for (int i = 0; i < newSnapPoints.Length; i++) {
					if (i < snapPoints.Length)
						newSnapPoints[i] = snapPoints[i];
					else
						newSnapPoints[i] = scriptTarget.getClampedPosition(new Vector3(0, 0, 0));
				}
				
				snapPoints = newSnapPoints;
			}
			
			for (int i = 0; i < snapPoints.Length; i++) {
				snapPoints[i] = EditorGUILayout.Vector3Field("" + i, snapPoints[i]);
			}
			
			EditorGUI.indentLevel--;
			
			scriptTarget.snapTo = snapPoints;
		}
		
		allowX = EditorGUILayout.Toggle("Translate X", allowX);
		if (allowX) {
			EditorGUI.indentLevel++;
			scriptTarget.minX = EditorGUILayout.FloatField("Min X", scriptTarget.minX);
			scriptTarget.maxX = EditorGUILayout.FloatField("Max X", scriptTarget.maxX);
			
			Vector2 d = EditorGUILayout.Vector2Field("dX per dMouse", new Vector2(scriptTarget.dXPerDMouseX, scriptTarget.dXPerDMouseY));
			scriptTarget.dXPerDMouseX = d.x;
			scriptTarget.dXPerDMouseY = d.y;
			EditorGUI.indentLevel--;
		} else {
			scriptTarget.minX = (scriptTarget.maxX = scriptTarget.transform.localPosition.x);
		}
		
		allowY = EditorGUILayout.Toggle("Translate Y", allowY);
		if (allowY) {
			EditorGUI.indentLevel++;
			scriptTarget.minY = EditorGUILayout.FloatField("Min Y", scriptTarget.minY);
			scriptTarget.maxY = EditorGUILayout.FloatField("Max Y", scriptTarget.maxY);
			
			Vector2 d = EditorGUILayout.Vector2Field("dY per dMouse", new Vector2(scriptTarget.dYPerDMouseX, scriptTarget.dYPerDMouseY));
			scriptTarget.dYPerDMouseX = d.x;
			scriptTarget.dYPerDMouseY = d.y;
			EditorGUI.indentLevel--;
		} else {
			scriptTarget.minY = (scriptTarget.maxY = scriptTarget.transform.localPosition.y);
		}
		
		allowZ = EditorGUILayout.Toggle("Translate Z", allowZ);
		if (allowZ) {
			EditorGUI.indentLevel++;
			scriptTarget.minZ = EditorGUILayout.FloatField("Min Z", scriptTarget.minZ);
			scriptTarget.maxZ = EditorGUILayout.FloatField("Max Z", scriptTarget.maxZ);
			
			Vector2 d = EditorGUILayout.Vector2Field("dZ per dMouse", new Vector2(scriptTarget.dZPerDMouseX, scriptTarget.dZPerDMouseY));
			scriptTarget.dZPerDMouseX = d.x;
			scriptTarget.dZPerDMouseY = d.y;
			EditorGUI.indentLevel--;
		} else {
			scriptTarget.minZ = (scriptTarget.maxZ = scriptTarget.transform.localPosition.z);
		}
		
        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }
}

