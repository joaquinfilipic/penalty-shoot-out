// Mono Framework
using System.Collections;

// Unity Framework
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Field))]
public class FieldInspector : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		Field fld = target as Field;

		if (GUILayout.Button("Redraw"))
		{
			fld.CalculateFieldMetrics();
			fld.CreateFieldMesh();
		}
		
		if (GUILayout.Button("Clear"))
		{
			fld.Clear();
		}
	}
}
