using System;
using UnityEditor;
using UnityEngine;
using AssemblyCSharp;

[CanEditMultipleObjects]
[CustomEditor(typeof(ClickAndDragRotate))]
public class ClickAndDragRotateEditor: Editor
{
	private ClickAndDragRotate scriptTarget;
	
	private bool allowX = false;
	private bool allowY = false;
	private bool allowZ = false;
	
	private bool snapOpen = false;
	private Vector3[] snapPoints;
	
	void OnEnable() {
		scriptTarget = (ClickAndDragRotate) target;
		
		if (scriptTarget.minX == 0.0f &&
			scriptTarget.maxX == 0.0f &&
			scriptTarget.minY == 0.0f &&
			scriptTarget.maxY == 0.0f &&
			scriptTarget.minZ == 0.0f &&
			scriptTarget.maxZ == 0.0f) {
			// this has never been edited
			
			scriptTarget.minX = scriptTarget.maxX = scriptTarget.transform.localRotation.x;
			scriptTarget.minY = scriptTarget.maxY = scriptTarget.transform.localRotation.y;
			scriptTarget.minZ = scriptTarget.maxZ = scriptTarget.transform.localRotation.z;
		}
		
		if (scriptTarget.snapTo == null)
			scriptTarget.snapTo = new Quaternion[] { scriptTarget.getClampedRotation(Quaternion.Euler(new Vector3(0, 0, 0))) };
		
		updateEditorSnapPoints();
		
		Vector3 r = round(scriptTarget.transform.localRotation.eulerAngles);
		allowX = !(r.x == scriptTarget.minX && r.x == scriptTarget.maxX);
		allowY = !(r.y == scriptTarget.minY && r.y == scriptTarget.maxY);
		allowZ = !(r.z == scriptTarget.minZ && r.z == scriptTarget.maxZ);
	}
	
	public override void OnInspectorGUI() {
		scriptTarget.gameStateKey = EditorGUILayout.TextField("Game State Key", scriptTarget.gameStateKey);
		scriptTarget.isActive = EditorGUILayout.Toggle("Active", scriptTarget.isActive);
		scriptTarget.maxDistance = EditorGUILayout.FloatField("Max Interaction Distane", scriptTarget.maxDistance);
		
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
						newSnapPoints[i] = scriptTarget.getClampedRotation(Quaternion.Euler(new Vector3(0, 0, 0))).eulerAngles;
				}
				
				snapPoints = newSnapPoints;
			}
			
			for (int i = 0; i < snapPoints.Length; i++) {
				snapPoints[i] = EditorGUILayout.Vector3Field("" + i, snapPoints[i]);
			}
			
			EditorGUI.indentLevel--;
			
			updateScriptSnapPoints();
		}
		
		allowX = EditorGUILayout.Toggle("Rotate X", allowX);
		if (allowX) {
			EditorGUI.indentLevel++;
			scriptTarget.clampX = EditorGUILayout.Toggle("Limit X Rotation", scriptTarget.clampX);
			if (scriptTarget.clampX) {
				scriptTarget.minX = EditorGUILayout.FloatField("Min X", scriptTarget.minX);
				scriptTarget.maxX = EditorGUILayout.FloatField("Max X", scriptTarget.maxX);
			}
			
			Vector2 d = EditorGUILayout.Vector2Field("dX per dMouse", new Vector2(scriptTarget.dXPerDMouseX, scriptTarget.dXPerDMouseY));
			scriptTarget.dXPerDMouseX = d.x;
			scriptTarget.dXPerDMouseY = d.y;
			EditorGUI.indentLevel--;
		} else {
			scriptTarget.minX = (scriptTarget.maxX = scriptTarget.transform.localRotation.x);
		}
		
		allowY = EditorGUILayout.Toggle("Rotate Y", allowY);
		if (allowY) {
			EditorGUI.indentLevel++;
			scriptTarget.clampY = EditorGUILayout.Toggle("Limit Y Rotation", scriptTarget.clampY);
			if (scriptTarget.clampY) {
				scriptTarget.minY = EditorGUILayout.FloatField("Min Y", scriptTarget.minY);
				scriptTarget.maxY = EditorGUILayout.FloatField("Max Y", scriptTarget.maxY);
			}
			
			Vector2 d = EditorGUILayout.Vector2Field("dY per dMouse", new Vector2(scriptTarget.dYPerDMouseX, scriptTarget.dYPerDMouseY));
			scriptTarget.dYPerDMouseX = d.x;
			scriptTarget.dYPerDMouseY = d.y;
			EditorGUI.indentLevel--;
		} else {
			scriptTarget.minY = (scriptTarget.maxY = scriptTarget.transform.localRotation.y);
		}
		
		allowZ = EditorGUILayout.Toggle("Rotate Z", allowZ);
		if (allowZ) {
			EditorGUI.indentLevel++;
			scriptTarget.clampZ = EditorGUILayout.Toggle("Limit Z Rotation", scriptTarget.clampZ);
			if (scriptTarget.clampZ) {
				scriptTarget.minZ = EditorGUILayout.FloatField("Min Z", scriptTarget.minZ);
				scriptTarget.maxZ = EditorGUILayout.FloatField("Max Z", scriptTarget.maxZ);
			}
			
			Vector2 d = EditorGUILayout.Vector2Field("dZ per dMouse", new Vector2(scriptTarget.dZPerDMouseX, scriptTarget.dZPerDMouseY));
			scriptTarget.dZPerDMouseX = d.x;
			scriptTarget.dZPerDMouseY = d.y;
			EditorGUI.indentLevel--;
		} else {
			scriptTarget.minZ = (scriptTarget.maxZ = scriptTarget.transform.localRotation.z);
		}
		
        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }
	
	private void updateScriptSnapPoints() {
		scriptTarget.snapTo = new Quaternion[snapPoints.Length];
		for (int i = 0; i < snapPoints.Length; i++) {
			scriptTarget.snapTo[i] = Quaternion.Euler(snapPoints[i]);
		}
	}
	
	private void updateEditorSnapPoints() {
		snapPoints = new Vector3[scriptTarget.snapTo.Length];
		for (int i = 0; i < snapPoints.Length; i++) {
			Vector3 point = scriptTarget.snapTo[i].eulerAngles;
			snapPoints[i] = point;
		}
	}
	
	private Vector3 round(Vector3 rotation) {
		float x = (float) decimal.Round((decimal) rotation.x, 2, MidpointRounding.AwayFromZero);
		float y = (float) decimal.Round((decimal) rotation.y, 2, MidpointRounding.AwayFromZero);
		float z = (float) decimal.Round((decimal) rotation.z, 2, MidpointRounding.AwayFromZero);
		return new Vector3(x, y, z);
	}
}

