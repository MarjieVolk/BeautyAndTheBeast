using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ClickAndDragTranslate))]
public class ClickAndDragTranslateEditor : Editor
{
	private ClickAndDragTranslate scriptTarget;
	
	private bool allowX = false;
	private bool allowY = false;
	private bool allowZ = false;
	
	void OnEnable() {
		scriptTarget = (ClickAndDragTranslate) target;
		
		if (scriptTarget.minX == 0.0f &&
			scriptTarget.maxX == 0.0f &&
			scriptTarget.minY == 0.0f &&
			scriptTarget.maxY == 0.0f &&
			scriptTarget.minZ == 0.0f &&
			scriptTarget.maxZ == 0.0f) {
			// this has never been edited
			return;
		}
		
		Vector3 p = scriptTarget.transform.localPosition;
		allowX = !(p.x == scriptTarget.minX && p.x == scriptTarget.maxX);
		allowY = !(p.y == scriptTarget.minY && p.y == scriptTarget.maxY);
		allowZ = !(p.z == scriptTarget.minZ && p.z == scriptTarget.maxZ);
	}
	
	public override void OnInspectorGUI() {
		scriptTarget.gameStateKey = EditorGUILayout.TextField("Game State Key", scriptTarget.gameStateKey);
		
		//TODO: snapTo
		
		allowX = EditorGUILayout.Toggle("Translate Along X-Axis", allowX);
		if (allowX) {
			EditorGUI.indentLevel++;
			EditorGUILayout.BeginHorizontal();
			scriptTarget.minX = EditorGUILayout.FloatField("Min X", scriptTarget.minX);
			scriptTarget.maxX = EditorGUILayout.FloatField("Max X", scriptTarget.maxX);
			EditorGUILayout.EndHorizontal();
			
			Vector2 d = EditorGUILayout.Vector2Field("dX per dMouse", new Vector2(scriptTarget.dXPerDMouseX, scriptTarget.dXPerDMouseY));
			scriptTarget.dXPerDMouseX = d.x;
			scriptTarget.dXPerDMouseY = d.y;
			EditorGUI.indentLevel--;
		} else {
			scriptTarget.minX = (scriptTarget.maxX = scriptTarget.transform.localPosition.x);
		}
		
		allowY = EditorGUILayout.Toggle("Translate Along Y-Axis", allowY);
		if (allowY) {
			EditorGUI.indentLevel++;
			EditorGUILayout.BeginHorizontal();
			scriptTarget.minY = EditorGUILayout.FloatField("Min Y", scriptTarget.minY);
			scriptTarget.maxY = EditorGUILayout.FloatField("Max Y", scriptTarget.maxY);
			EditorGUILayout.EndHorizontal();
			
			Vector2 d = EditorGUILayout.Vector2Field("dY per dMouse", new Vector2(scriptTarget.dYPerDMouseX, scriptTarget.dYPerDMouseY));
			scriptTarget.dYPerDMouseX = d.x;
			scriptTarget.dYPerDMouseY = d.y;
			EditorGUI.indentLevel--;
		} else {
			scriptTarget.minY = (scriptTarget.maxY = scriptTarget.transform.localPosition.y);
		}
		
		allowZ = EditorGUILayout.Toggle("Translate Along Z-Axis", allowZ);
		if (allowZ) {
			EditorGUI.indentLevel++;
			EditorGUILayout.BeginHorizontal();
			scriptTarget.minZ = EditorGUILayout.FloatField("Min Z", scriptTarget.minZ);
			scriptTarget.maxZ = EditorGUILayout.FloatField("Max Z", scriptTarget.maxZ);
			EditorGUILayout.EndHorizontal();
			
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

