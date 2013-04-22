using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ClickAndDragRotateByMousePosition))]
public class ClickAndDragRotateByMousePositionEditor: Editor
{
	private static bool snapOpen = false;
	
	private ClickAndDragRotateByMousePosition scriptTarget;
	
	private Vector3[] snapPoints;
	private Vector3 dPerMouseX;
	private Vector3 dPerMouseY;
	
	void OnEnable() {
		scriptTarget = (ClickAndDragRotateByMousePosition) target;
		
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
		
		dPerMouseX = new Vector3(scriptTarget.dXPerDMouseX, scriptTarget.dYPerDMouseX, scriptTarget.dZPerDMouseX);
		dPerMouseY = new Vector3(scriptTarget.dXPerDMouseY, scriptTarget.dYPerDMouseY, scriptTarget.dZPerDMouseY);
	}
	
	public override void OnInspectorGUI() {
		scriptTarget.gameStateKey = EditorGUILayout.TextField("Game State Key", scriptTarget.gameStateKey);
		scriptTarget.isActive = EditorGUILayout.Toggle("Active", scriptTarget.isActive);
		scriptTarget.maxDistance = EditorGUILayout.FloatField("Max Interaction Distane", scriptTarget.maxDistance);
		scriptTarget.minDistance = EditorGUILayout.FloatField("Min Interaction Distane", scriptTarget.minDistance);
		scriptTarget.rotateSound = (AudioClip) EditorGUILayout.ObjectField("Sound Effect", scriptTarget.rotateSound, typeof(AudioClip), true);
		
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
		
		scriptTarget.clampX = EditorGUILayout.Toggle("Limit X Rotation", scriptTarget.clampX);
		if (scriptTarget.clampX) {
			EditorGUI.indentLevel++;
			scriptTarget.minX = EditorGUILayout.FloatField("Min X", scriptTarget.minX);
			scriptTarget.maxX = EditorGUILayout.FloatField("Max X", scriptTarget.maxX);
			EditorGUI.indentLevel--;
		}
		
		scriptTarget.clampY = EditorGUILayout.Toggle("Limit Y Rotation", scriptTarget.clampY);
		if (scriptTarget.clampY) {
			EditorGUI.indentLevel++;
			scriptTarget.minY = EditorGUILayout.FloatField("Min Y", scriptTarget.minY);
			scriptTarget.maxY = EditorGUILayout.FloatField("Max Y", scriptTarget.maxY);
			EditorGUI.indentLevel--;
		}
		
		scriptTarget.clampZ = EditorGUILayout.Toggle("Limit Z Rotation", scriptTarget.clampZ);
		if (scriptTarget.clampZ) {
			EditorGUI.indentLevel++;
			scriptTarget.minZ = EditorGUILayout.FloatField("Min Z", scriptTarget.minZ);
			scriptTarget.maxZ = EditorGUILayout.FloatField("Max Z", scriptTarget.maxZ);
			EditorGUI.indentLevel--;
		}
		
		dPerMouseX = EditorGUILayout.Vector3Field("Rotation Per Change in Mouse X", dPerMouseX);
		dPerMouseY = EditorGUILayout.Vector3Field("Rotation Per Change in Mouse Y", dPerMouseY);
		scriptTarget.dXPerDMouseX = dPerMouseX.x;
		scriptTarget.dYPerDMouseX = dPerMouseX.y;
		scriptTarget.dZPerDMouseX = dPerMouseX.z;
		scriptTarget.dXPerDMouseY = dPerMouseY.x;
		scriptTarget.dYPerDMouseY = dPerMouseY.y;
		scriptTarget.dZPerDMouseY = dPerMouseY.z;
		
        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }
	
	protected void updateScriptSnapPoints() {
		scriptTarget.snapTo = new Quaternion[snapPoints.Length];
		for (int i = 0; i < snapPoints.Length; i++) {
			scriptTarget.snapTo[i] = Quaternion.Euler(snapPoints[i]);
		}
	}
	
	protected void updateEditorSnapPoints() {
		snapPoints = new Vector3[scriptTarget.snapTo.Length];
		for (int i = 0; i < snapPoints.Length; i++) {
			Vector3 point = scriptTarget.snapTo[i].eulerAngles;
			snapPoints[i] = point;
		}
	}
	
	protected Vector3 round(Vector3 rotation) {
		float x = (float) decimal.Round((decimal) rotation.x, 2, MidpointRounding.AwayFromZero);
		float y = (float) decimal.Round((decimal) rotation.y, 2, MidpointRounding.AwayFromZero);
		float z = (float) decimal.Round((decimal) rotation.z, 2, MidpointRounding.AwayFromZero);
		return new Vector3(x, y, z);
	}
}

