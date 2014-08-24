using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(PlanetSpawner))]
[CanEditMultipleObjects]
public class PlanetSpawnerEditor : Editor
{

	// Members
	// -----------------------------------------------------
	
	private SerializedProperty prefabProp;
	private SerializedProperty radiusProp;
	private SerializedProperty minSpawnProp;
	private SerializedProperty maxSpawnProp;
	private SerializedProperty minScaleProp;
	private SerializedProperty maxScaleProp;

	
	// Unity Implementation
	// -----------------------------------------------------
	
	/** Handles the inspector being enabled. */
	public void OnEnable()
	{
		prefabProp = serializedObject.FindProperty("Prefab");	
		radiusProp = serializedObject.FindProperty("Radius");
		minSpawnProp = serializedObject.FindProperty("MinSpawn");
		maxSpawnProp = serializedObject.FindProperty("MaxSpawn");
		minScaleProp = serializedObject.FindProperty("MinScale");
		maxScaleProp = serializedObject.FindProperty("MaxScale");
	}
	
	/** Updates the Unity inspector GUI. */
	public override void OnInspectorGUI()
	{
		// Update the serialized property.
		serializedObject.Update();

		// Edit properties
		EditorGUILayout.PropertyField(prefabProp, new GUIContent("Prefab"));
		EditorGUILayout.PropertyField(radiusProp, new GUIContent("Radius"));
		EditorGUILayout.PropertyField(minSpawnProp, new GUIContent("Min Spawn"));
		EditorGUILayout.PropertyField(maxSpawnProp, new GUIContent("Max Spawn"));
		EditorGUILayout.PropertyField(minScaleProp, new GUIContent("Min Scale"));
		EditorGUILayout.PropertyField(maxScaleProp, new GUIContent("Max Scale"));

		// Generate objects on command.
		if (GUILayout.Button("Generate!", GUILayout.Height(24)))
			foreach (PlanetSpawner p in serializedObject.targetObjects)
				p.Generate();

		GUILayout.Space(10);
		
		// Apply changes to edited object(s).
		serializedObject.ApplyModifiedProperties();
	}

}
