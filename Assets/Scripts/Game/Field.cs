// Mono Framework
using System.Collections.Generic;

// Unity Framework
using UnityEngine;

[ExecuteInEditMode]
public class Field : MonoBehaviorSingleton<Field>
{
	// Y Position of the field lines
	const float FieldYPos = 0.025f;

	// Length of the field
    public float length = 60.0f;

	// Width of the field
    public float width = 20.0f;

    public float circleWidthPerc = 0.5f;
	
	// Line width
	public float lineWidth = 0.1f;

	// Area width in relation of the width of the field
	public float areaWidthPerc = 0.75f;

    // Area length in relation of the length of the field
	public float areaLengthPerc = 0.3f;

    // Goal width in relation of the width of the field
    public float goalWidthPerc = 0.3f;

    // Goal lenght in relation of the length of the field
    public float goalLengthPerc = 0.09f;

	// Goal width (absolute value). This value is hardcoded. All the goals variations should have the same measures.
	public float goalWidth = 8.0f;

	// Goal height (absolute value). This value is hardcoded. All the goals variations should have the same measures.
	public float goalHeight = 3.05f;

	// Area width in relation of the width of the field
	public float smallAreaWidthPerc = 0.5f;
	
    // Area length in relation of the length of the field
	public float smallAreaLengthPerc = 0.1f;

	// Line Material
	public Material lineMaterial;

    public float penaltyDistanceFromGoal = 5f;

	// Corner Points
	Vector3[] corners;

	// Half line
	Vector3[] halfLine;

	// Area 1 points
	Vector3[] area1;

	// Area 2 points
	Vector3[] area2;

	// Small area 1 points
	Vector3[] area1s;

	// Small area 2 points
	Vector3[] area2s;

	// Center point of the field
	Vector3 centerPoint;

	// Penalty point 1
	Vector3 penaltyPoint1;

	// Penalty point 2
	Vector3 penaltyPoint2;

	// Mesh of the lines of the field
	Mesh linesMesh;

    // Calculated
    Rect fieldBounds;

    Rect areaTeam1Bounds;
    Rect areaTeam2Bounds;

    // Penalty position
	float penaltyZPos;

	// Unity Awake Method
	new void Awake()
	{
		base.Awake();

		CalculateFieldMetrics();
		CreateFieldMesh();
	}

	public void Clear()
	{
		linesMesh = null;
		GetComponent<MeshFilter>().mesh = linesMesh;
	}

	public void CalculateFieldMetrics()
	{
		UpdateBounds();
		
		centerPoint = new Vector3(0.0f, FieldYPos, 0.0f);

		penaltyZPos = (length / 2.0f) - (length * areaLengthPerc) + penaltyDistanceFromGoal;
		penaltyPoint1 = new Vector3(0.0f, FieldYPos, penaltyZPos);
		penaltyPoint2 = new Vector3(0.0f, FieldYPos, -penaltyZPos);
	}
	
    void DrawField(out Vector3[] verts, out int[] tris, out Color[] cols)
    {
		float lineWidthDiv2 = lineWidth / 2.0f;

		#region Index Constants
		const int p1a = 0;
		const int p1b = 1;
		const int p1c = 2;
		const int p1d = 3;
		const int p2a = 4;
		const int p2b = 5;
		const int p3a = 6;
		const int p3b = 7;
		const int p4a = 8;
		const int p4b = 9;
		const int p4c = 10;
		const int p4d = 11;

		const int p5a = 12;
		const int p5b = 13;
		const int p5c = 14;
		const int p5d = 15;
		const int p5e = 16;
		const int p5f = 17;
		const int p5g = 18;
		const int p5h = 19;

		const int p6a = 20;
		const int p6b = 21;
		const int p6c = 22;
		const int p6d = 23;
		const int p6e = 24;
		const int p6f = 25;
		const int p6g = 26;
		const int p6h = 27;

		const int p7a = 28;
		const int p7b = 29;
		const int p7c = 30;
		const int p7d = 31;
		const int p7e = 32;
		const int p7f = 33;
		const int p7g = 34;
		const int p7h = 35;

		const int p8a = 36;
		const int p8b = 37;
		const int p8c = 38;
		const int p8d = 39;
		const int p8e = 40;
		const int p8f = 41;
		const int p8g = 42;
		const int p8h = 43;

		const int p9a = 44;
		const int p9b = 45;
		const int p9c = 46;
		const int p9d = 47;

		const int p10a = 48;
		const int p10b = 49;
		const int p10c = 50;
		const int p10d = 51;

		const int p11a = 52;
		const int p11b = 53;
		const int p11c = 54;
		const int p11d = 55;
		#endregion

		corners = new Vector3[4];
		corners[0] = new Vector3(width / -2.0f, FieldYPos, length / 2.0f);
		corners[1] = new Vector3(width / 2.0f, FieldYPos, length / 2.0f);
		corners[2] = new Vector3(width / 2.0f, FieldYPos, length / -2.0f);
		corners[3] = new Vector3(width / -2.0f, FieldYPos, length / -2.0f);

		halfLine = new Vector3[2];
		halfLine[0] = new Vector3(width / -2.0f, FieldYPos, 0.0f);
		halfLine[1] = new Vector3(width / 2.0f, FieldYPos, 0.0f);

		area1 = new Vector3[4];
		area1[0] = new Vector3((width * areaWidthPerc) / -2.0f, FieldYPos, length / 2.0f);
		area1[1] = new Vector3((width * areaWidthPerc) / 2.0f, FieldYPos, length / 2.0f);
		area1[2] = new Vector3((width * areaWidthPerc) / 2.0f, FieldYPos, (length / 2.0f) - (length * areaLengthPerc));
		area1[3] = new Vector3((width * areaWidthPerc) / -2.0f, FieldYPos, (length / 2.0f) - (length * areaLengthPerc));

		area2 = new Vector3[4];
		area2[0] = new Vector3((width * areaWidthPerc) / 2.0f, FieldYPos, length / -2.0f);
		area2[1] = new Vector3((width * areaWidthPerc) / -2.0f, FieldYPos, length / -2.0f);
		area2[2] = new Vector3((width * areaWidthPerc) / -2.0f, FieldYPos, (length / -2.0f) + (length * areaLengthPerc));
		area2[3] = new Vector3((width * areaWidthPerc) / 2.0f, FieldYPos, (length / -2.0f) + (length * areaLengthPerc));

		area1s = new Vector3[4];
		area1s[0] = new Vector3((width * smallAreaWidthPerc) / -2.0f, FieldYPos, length / 2.0f);
		area1s[1] = new Vector3((width * smallAreaWidthPerc) / 2.0f, FieldYPos, length / 2.0f);
		area1s[2] = new Vector3((width * smallAreaWidthPerc) / 2.0f, FieldYPos, (length / 2.0f) - (length * smallAreaLengthPerc));
		area1s[3] = new Vector3((width * smallAreaWidthPerc) / -2.0f, FieldYPos, (length / 2.0f) - (length * smallAreaLengthPerc));

		area2s = new Vector3[4];
		area2s[0] = new Vector3((width * smallAreaWidthPerc) / 2.0f, FieldYPos, length / -2.0f);
		area2s[1] = new Vector3((width * smallAreaWidthPerc) / -2.0f, FieldYPos, length / -2.0f);
		area2s[2] = new Vector3((width * smallAreaWidthPerc) / -2.0f, FieldYPos, (length / -2.0f) + (length * smallAreaLengthPerc));
		area2s[3] = new Vector3((width * smallAreaWidthPerc) / 2.0f, FieldYPos, (length / -2.0f) + (length * smallAreaLengthPerc));

		const int vertCount = 56;

		verts = new Vector3[vertCount];
		// Corner sub-points
		verts[p1a] = new Vector3(corners[0].x - lineWidthDiv2, FieldYPos, corners[0].z + lineWidthDiv2); // 1a
		verts[p1b] = new Vector3(corners[1].x + lineWidthDiv2, FieldYPos, corners[1].z + lineWidthDiv2); // 1b
		verts[p1c] = new Vector3(corners[1].x - lineWidthDiv2, FieldYPos, corners[1].z - lineWidthDiv2); // 1c
		verts[p1d] = new Vector3(corners[0].x + lineWidthDiv2, FieldYPos, corners[0].z - lineWidthDiv2); // 1d
		verts[p2a] = new Vector3(corners[2].x + lineWidthDiv2, FieldYPos, corners[2].z - lineWidthDiv2); // 2a
		verts[p2b] = new Vector3(corners[2].x - lineWidthDiv2, FieldYPos, corners[2].z + lineWidthDiv2); // 2b
		verts[p3a] = new Vector3(corners[3].x - lineWidthDiv2, FieldYPos, corners[3].z - lineWidthDiv2); // 3a
		verts[p3b] = new Vector3(corners[3].x + lineWidthDiv2, FieldYPos, corners[3].z + lineWidthDiv2); // 3b

		// Half Line sub-points
		verts[p4a] = new Vector3(halfLine[0].x + lineWidthDiv2, FieldYPos, halfLine[0].z + lineWidthDiv2); // 4a
		verts[p4b] = new Vector3(halfLine[1].x - lineWidthDiv2, FieldYPos, halfLine[1].z + lineWidthDiv2); // 4b
		verts[p4c] = new Vector3(halfLine[1].x - lineWidthDiv2, FieldYPos, halfLine[1].z - lineWidthDiv2); // 4c
		verts[p4d] = new Vector3(halfLine[0].x + lineWidthDiv2, FieldYPos, halfLine[0].z - lineWidthDiv2); // 4d

		// Area 1
		verts[p5a] = new Vector3(area1[0].x - lineWidthDiv2, FieldYPos, area1[0].z + lineWidthDiv2); // 5a
		verts[p5b] = new Vector3(area1[0].x + lineWidthDiv2, FieldYPos, area1[0].z + lineWidthDiv2); // 5b
		verts[p5c] = new Vector3(area1[1].x - lineWidthDiv2, FieldYPos, area1[1].z + lineWidthDiv2); // 5c
		verts[p5d] = new Vector3(area1[1].x + lineWidthDiv2, FieldYPos, area1[1].z + lineWidthDiv2); // 5d
		verts[p5e] = new Vector3(area1[2].x + lineWidthDiv2, FieldYPos, area1[2].z - lineWidthDiv2); // 5e
		verts[p5f] = new Vector3(area1[2].x - lineWidthDiv2, FieldYPos, area1[2].z + lineWidthDiv2); // 5f
		verts[p5g] = new Vector3(area1[3].x - lineWidthDiv2, FieldYPos, area1[3].z - lineWidthDiv2); // 5g
		verts[p5h] = new Vector3(area1[3].x + lineWidthDiv2, FieldYPos, area1[3].z + lineWidthDiv2); // 5h

		// Area 2
		verts[p6a] = new Vector3(area2[0].x + lineWidthDiv2, FieldYPos, area2[0].z - lineWidthDiv2); // 6a
		verts[p6b] = new Vector3(area2[0].x - lineWidthDiv2, FieldYPos, area2[0].z - lineWidthDiv2); // 6b
		verts[p6c] = new Vector3(area2[1].x + lineWidthDiv2, FieldYPos, area2[1].z - lineWidthDiv2); // 6c
		verts[p6d] = new Vector3(area2[1].x - lineWidthDiv2, FieldYPos, area2[1].z - lineWidthDiv2); // 6d
		verts[p6e] = new Vector3(area2[2].x - lineWidthDiv2, FieldYPos, area2[2].z + lineWidthDiv2); // 6e
		verts[p6f] = new Vector3(area2[2].x + lineWidthDiv2, FieldYPos, area2[2].z - lineWidthDiv2); // 6f
		verts[p6g] = new Vector3(area2[3].x + lineWidthDiv2, FieldYPos, area2[3].z + lineWidthDiv2); // 6g
		verts[p6h] = new Vector3(area2[3].x - lineWidthDiv2, FieldYPos, area2[3].z - lineWidthDiv2); // 6h

		// Small Area 1
		verts[p7a] = new Vector3(area1s[0].x - lineWidthDiv2, FieldYPos, area1s[0].z + lineWidthDiv2); // 7a
		verts[p7b] = new Vector3(area1s[0].x + lineWidthDiv2, FieldYPos, area1s[0].z + lineWidthDiv2); // 7b
		verts[p7c] = new Vector3(area1s[1].x - lineWidthDiv2, FieldYPos, area1s[1].z + lineWidthDiv2); // 7c
		verts[p7d] = new Vector3(area1s[1].x + lineWidthDiv2, FieldYPos, area1s[1].z + lineWidthDiv2); // 7d
		verts[p7e] = new Vector3(area1s[2].x + lineWidthDiv2, FieldYPos, area1s[2].z - lineWidthDiv2); // 7e
		verts[p7f] = new Vector3(area1s[2].x - lineWidthDiv2, FieldYPos, area1s[2].z + lineWidthDiv2); // 7f
		verts[p7g] = new Vector3(area1s[3].x - lineWidthDiv2, FieldYPos, area1s[3].z - lineWidthDiv2); // 7g
		verts[p7h] = new Vector3(area1s[3].x + lineWidthDiv2, FieldYPos, area1s[3].z + lineWidthDiv2); // 7h

		// Small Area 2
		verts[p8a] = new Vector3(area2s[0].x + lineWidthDiv2, FieldYPos, area2s[0].z - lineWidthDiv2); // 8a
		verts[p8b] = new Vector3(area2s[0].x - lineWidthDiv2, FieldYPos, area2s[0].z - lineWidthDiv2); // 8b
		verts[p8c] = new Vector3(area2s[1].x + lineWidthDiv2, FieldYPos, area2s[1].z - lineWidthDiv2); // 8c
		verts[p8d] = new Vector3(area2s[1].x - lineWidthDiv2, FieldYPos, area2s[1].z - lineWidthDiv2); // 8d
		verts[p8e] = new Vector3(area2s[2].x - lineWidthDiv2, FieldYPos, area2s[2].z + lineWidthDiv2); // 8e
		verts[p8f] = new Vector3(area2s[2].x + lineWidthDiv2, FieldYPos, area2s[2].z - lineWidthDiv2); // 8f
		verts[p8g] = new Vector3(area2s[3].x + lineWidthDiv2, FieldYPos, area2s[3].z + lineWidthDiv2); // 8g
		verts[p8h] = new Vector3(area2s[3].x - lineWidthDiv2, FieldYPos, area2s[3].z - lineWidthDiv2); // 8h

		// Center Point
		verts[p9a] = new Vector3(centerPoint.x - lineWidth, FieldYPos, centerPoint.z + lineWidth); // 9a
		verts[p9b] = new Vector3(centerPoint.x + lineWidth, FieldYPos, centerPoint.z + lineWidth); // 9b
		verts[p9c] = new Vector3(centerPoint.x + lineWidth, FieldYPos, centerPoint.z - lineWidth); // 9c
		verts[p9d] = new Vector3(centerPoint.x - lineWidth, FieldYPos, centerPoint.z - lineWidth); // 9d

		// Penalty Point 1
		verts[p10a] = new Vector3(penaltyPoint1.x - lineWidth, FieldYPos, penaltyPoint1.z + lineWidth); // 10a
		verts[p10b] = new Vector3(penaltyPoint1.x + lineWidth, FieldYPos, penaltyPoint1.z + lineWidth); // 10b
		verts[p10c] = new Vector3(penaltyPoint1.x + lineWidth, FieldYPos, penaltyPoint1.z - lineWidth); // 10c
		verts[p10d] = new Vector3(penaltyPoint1.x - lineWidth, FieldYPos, penaltyPoint1.z - lineWidth); // 10d

		// Penalty Point 2
		verts[p11a] = new Vector3(penaltyPoint2.x - lineWidth, FieldYPos, penaltyPoint2.z + lineWidth); // 11a
		verts[p11b] = new Vector3(penaltyPoint2.x + lineWidth, FieldYPos, penaltyPoint2.z + lineWidth); // 11b
		verts[p11c] = new Vector3(penaltyPoint2.x + lineWidth, FieldYPos, penaltyPoint2.z - lineWidth); // 11c
		verts[p11d] = new Vector3(penaltyPoint2.x - lineWidth, FieldYPos, penaltyPoint2.z - lineWidth); // 11d

		tris = new int[120];
		int idx = 0;
		// Line 1
		tris[idx++] = p1a; tris[idx++] = p1b; tris[idx++] = p1d; // 1a / 1b / 1d
		tris[idx++] = p1d; tris[idx++] = p1b; tris[idx++] = p1c; // 1d / 1b / 1c

		// Line 2
		tris[idx++] = p1b; tris[idx++] = p2a; tris[idx++] = p1c; // 1b / 2a / 1c
		tris[idx++] = p1c; tris[idx++] = p2a; tris[idx++] = p2b; // 1c / 2a / 2b

		// Line 3
		tris[idx++] = p3b; tris[idx++] = p2b; tris[idx++] = p3a; // 3b / 2b / 3a
		tris[idx++] = p3a; tris[idx++] = p2b; tris[idx++] = p2a; // 3a / 2b / 2a

		// Line 4
		tris[idx++] = p1a; tris[idx++] = p1d; tris[idx++] = p3a; // 1a / 1d / 3a
		tris[idx++] = p3a; tris[idx++] = p1d; tris[idx++] = p3b; // 3a / 1d / 3b

		// Middle Line
		tris[idx++] = p4a; tris[idx++] = p4b; tris[idx++] = p4d; // 4a / 4b / 4d
		tris[idx++] = p4d; tris[idx++] = p4b; tris[idx++] = p4c; // 4d / 4b / 4c

		// Area 1
		tris[idx++] = p5b; tris[idx++] = p5h; tris[idx++] = p5a; // 5b / 5h / 5a
		tris[idx++] = p5a; tris[idx++] = p5h; tris[idx++] = p5g; // 5a / 5h / 5g
		tris[idx++] = p5d; tris[idx++] = p5e; tris[idx++] = p5c; // 5d / 5e / 5c
		tris[idx++] = p5c; tris[idx++] = p5e; tris[idx++] = p5f; // 5c / 5e / 5f
		tris[idx++] = p5h; tris[idx++] = p5f; tris[idx++] = p5g; // 5h / 5f / 5g
		tris[idx++] = p5g; tris[idx++] = p5f; tris[idx++] = p5e; // 5g / 5f / 5e

		// Area 2
		tris[idx++] = p6b; tris[idx++] = p6h; tris[idx++] = p6a; // 5b / 5h / 5a
		tris[idx++] = p6a; tris[idx++] = p6h; tris[idx++] = p6g; // 5a / 5h / 5g
		tris[idx++] = p6d; tris[idx++] = p6e; tris[idx++] = p6c; // 5d / 5e / 5c
		tris[idx++] = p6c; tris[idx++] = p6e; tris[idx++] = p6f; // 5c / 5e / 5f
		tris[idx++] = p6h; tris[idx++] = p6f; tris[idx++] = p6g; // 5h / 5f / 5g
		tris[idx++] = p6g; tris[idx++] = p6f; tris[idx++] = p6e; // 5g / 5f / 5e

		// Small Area 1
		tris[idx++] = p7b; tris[idx++] = p7h; tris[idx++] = p7a; // 7b / 7h / 7a
		tris[idx++] = p7a; tris[idx++] = p7h; tris[idx++] = p7g; // 7a / 7h / 7g
		tris[idx++] = p7d; tris[idx++] = p7e; tris[idx++] = p7c; // 7d / 7e / 7c
		tris[idx++] = p7c; tris[idx++] = p7e; tris[idx++] = p7f; // 7c / 7e / 7f
		tris[idx++] = p7h; tris[idx++] = p7f; tris[idx++] = p7g; // 7h / 7f / 7g
		tris[idx++] = p7g; tris[idx++] = p7f; tris[idx++] = p7e; // 7g / 7f / 7e

		// Small Area 2
		tris[idx++] = p8b; tris[idx++] = p8h; tris[idx++] = p8a; // 8b / 8h / 8a
		tris[idx++] = p8a; tris[idx++] = p8h; tris[idx++] = p8g; // 8a / 8h / 8g
		tris[idx++] = p8d; tris[idx++] = p8e; tris[idx++] = p8c; // 8d / 8e / 8c
		tris[idx++] = p8c; tris[idx++] = p8e; tris[idx++] = p8f; // 8c / 8e / 8f
		tris[idx++] = p8h; tris[idx++] = p8f; tris[idx++] = p8g; // 8h / 8f / 8g
		tris[idx++] = p8g; tris[idx++] = p8f; tris[idx++] = p8e; // 8g / 8f / 8e

		// Center Point
		tris[idx++] = p9b; tris[idx++] = p9c; tris[idx++] = p9a; // 9b / 9c / 9a
		tris[idx++] = p9a; tris[idx++] = p9c; tris[idx++] = p9d; // 9a / 9c / 9d

		// Penalty Point 1
		tris[idx++] = p10b; tris[idx++] = p10c; tris[idx++] = p10a; // 10b / 10c / 10a
		tris[idx++] = p10a; tris[idx++] = p10c; tris[idx++] = p10d; // 10a / 10c / 10d

		// Penalty Point 2
		tris[idx++] = p11b; tris[idx++] = p11c; tris[idx++] = p11a; // 11b / 11c / 11a
		tris[idx++] = p11a; tris[idx++] = p11c; tris[idx++] = p11d; // 11a / 11c / 11d

		cols = new Color[vertCount];
		for (int i = 0; i < cols.Length; i++)
		{
			cols[i] = Color.white;
		}
    }

	/// <summary>
	/// Create the mesh of the lines of the field
	/// </summary>
	public void CreateFieldMesh()
	{
        Vector3[] vertsField;
        int[] trisField;
        Color[] colsField;
        DrawField(out vertsField, out trisField, out colsField);

		Vector3[] vertsCircle;
		int[] trisCircle;
		Color[] colsCircle;
        DrawCentralCircle(out vertsCircle, out trisCircle, out colsCircle);

		Vector3[] vertsSemiCircle;
		int[] trisSemiCircle;
		Color[] colsSemiCircle;
        DrawSemiCircle(out vertsSemiCircle, out trisSemiCircle, out colsSemiCircle, 1);

		Vector3[] vertsSemiCircle2;
		int[] trisSemiCircle2;
		Color[] colsSemiCircle2;
		DrawSemiCircle(out vertsSemiCircle2, out trisSemiCircle2, out colsSemiCircle2, -1);

        Vector3[] allVerts = new Vector3[vertsField.Length + vertsCircle.Length + vertsSemiCircle.Length + vertsSemiCircle2.Length];
        vertsField.CopyTo(allVerts, 0);
        vertsCircle.CopyTo(allVerts, vertsField.Length);
        vertsSemiCircle.CopyTo(allVerts, vertsField.Length + vertsCircle.Length);
        vertsSemiCircle2.CopyTo(allVerts, vertsField.Length + vertsCircle.Length + vertsSemiCircle.Length);

        Color[] allCols = new Color[colsField.Length + colsCircle.Length + colsSemiCircle.Length + colsSemiCircle2.Length];
		colsField.CopyTo(allCols, 0);
		colsCircle.CopyTo(allCols, colsField.Length);
        colsSemiCircle.CopyTo(allCols, colsField.Length + colsCircle.Length);
        colsSemiCircle2.CopyTo(allCols, colsField.Length + colsCircle.Length + colsSemiCircle.Length);

        int[] allTris = new int[trisField.Length + trisCircle.Length + trisSemiCircle.Length + trisSemiCircle2.Length];
		trisField.CopyTo(allTris, 0);

        for (int i = 0; i < trisCircle.Length; i++)
        {
            allTris[trisField.Length + i] = vertsField.Length + trisCircle[i];
        }

		for (int i = 0; i < trisSemiCircle.Length; i++)
		{
			allTris[trisField.Length + trisCircle.Length + i] = vertsField.Length + vertsCircle.Length + trisSemiCircle[i];
		}

		for (int i = 0; i < trisSemiCircle2.Length; i++)
		{
            allTris[trisField.Length + trisCircle.Length + trisSemiCircle2.Length + i] = vertsField.Length + vertsCircle.Length + vertsSemiCircle.Length + trisSemiCircle2[i];
		}

        linesMesh = new Mesh();

		linesMesh.vertices = allVerts;
		linesMesh.colors = allCols;
		linesMesh.triangles = allTris;

		linesMesh.RecalculateNormals();

		GetComponent<MeshFilter>().mesh = linesMesh;

	}

    void DrawSemiCircle(out Vector3[] verts, out int[] tris, out Color[] cols, int side = 1)
    {
        float ofsy = - penaltyZPos;

		const float stepInc = 2; // deg
		float yPos, xPos;

		float rad = width * circleWidthPerc;
		float radSq = rad * rad;

        float angleFrom = -60f;
        float angleTo = 60f;
		List<KeyValuePair<float, float>> extVerts = new List<KeyValuePair<float, float>>();
		for (float i = angleFrom; i <= angleTo; i += stepInc)
		{
            xPos = Mathf.Sin(i * Mathf.Deg2Rad) * rad;
			yPos = Mathf.Sqrt(radSq - Mathf.Pow(xPos, 2));
			extVerts.Add(new KeyValuePair<float, float>(xPos, yPos));
		}

		float rad2 = rad - lineWidth;
		float rad2Sq = rad2 * rad2;

		List<KeyValuePair<float, float>> innVerts = new List<KeyValuePair<float, float>>();

		for (float i = angleFrom; i <= angleTo; i += stepInc)
		{
			xPos = Mathf.Sin(i * Mathf.Deg2Rad) * rad2;
			yPos = Mathf.Sqrt(rad2Sq - Mathf.Pow(xPos, 2));
			innVerts.Add(new KeyValuePair<float, float>(xPos, yPos));
		}

		verts = new Vector3[extVerts.Count + innVerts.Count];
		for (int i = 0; i < extVerts.Count; i++)
		{
            verts[i * 2] = new Vector3(extVerts[i].Key, FieldYPos, (extVerts[i].Value + ofsy) * side);
            verts[i * 2 + 1] = new Vector3(innVerts[i].Key, FieldYPos, (innVerts[i].Value + ofsy) * side);
		}

		tris = new int[extVerts.Count * 6];

		for (int i = 0; i < extVerts.Count - 1; i++)
		{
            if (side > 0)
            {
                tris[i * 6] = i * 2 + 0; tris[i * 6 + 1] = i * 2 + 2; tris[i * 6 + 2] = i * 2 + 1;
                tris[i * 6 + 3] = i * 2 + 1; tris[i * 6 + 4] = i * 2 + 2; tris[i * 6 + 5] = i * 2 + 3;
            }
            else
            {
                tris[i * 6] = i * 2 + 0; tris[i * 6 + 1] = i * 2 + 1; tris[i * 6 + 2] = i * 2 + 2;
                tris[i * 6 + 3] = i * 2 + 2; tris[i * 6 + 4] = i * 2 + 1; tris[i * 6 + 5] = i * 2 + 3;
            }
		}

		cols = new Color[verts.Length];
		for (int i = 0; i < cols.Length; i++)
		{
			cols[i] = Color.white;
		}
    }

    void DrawCentralCircle(out Vector3[] verts, out int[] tris, out Color[] cols)
    {
        float stepInc = 5f; // def
		float yPos, xPos;

        float rad = width * circleWidthPerc;
        float radSq = rad * rad;

        List<KeyValuePair<float, float>> extVerts = new List<KeyValuePair<float, float>>();

        float angleFrom = -90f;
		float angleTo = 90f;
		for (float i = angleFrom; i <= angleTo; i += stepInc)
		{
			xPos = Mathf.Sin(i * Mathf.Deg2Rad) * rad;
            yPos = Mathf.Sqrt(radSq - Mathf.Pow(xPos, 2));
            extVerts.Add(new KeyValuePair<float, float>(xPos, yPos));
        }

        for (int i = extVerts.Count - 1; i >= 0; i--)
        {
            extVerts.Add(new KeyValuePair<float, float>(extVerts[i].Key, extVerts[i].Value * -1));
        }

		float rad2 = rad - lineWidth;
		float rad2Sq = rad2 * rad2;

		List<KeyValuePair<float, float>> innVerts = new List<KeyValuePair<float, float>>();

		for (float i = angleFrom; i <= angleTo; i += stepInc)
		{
			xPos = Mathf.Sin(i * Mathf.Deg2Rad) * rad2;
			yPos = Mathf.Sqrt(rad2Sq - Mathf.Pow(xPos, 2));
			innVerts.Add(new KeyValuePair<float, float>(xPos, yPos));
		}

		for (int i = innVerts.Count - 1; i >= 0; i--)
		{
			innVerts.Add(new KeyValuePair<float, float>(innVerts[i].Key, innVerts[i].Value * -1));
		}

		verts = new Vector3[extVerts.Count + innVerts.Count];
        for (int i = 0; i < extVerts.Count; i++)
        {
            verts[i * 2] = new Vector3(extVerts[i].Key, FieldYPos, extVerts[i].Value);
            verts[i * 2 + 1] = new Vector3(innVerts[i].Key, FieldYPos, innVerts[i].Value);
        }

        tris = new int[extVerts.Count * 6];

        for (int i = 0; i < extVerts.Count - 1; i++)
        {
             tris[i * 6] = i * 2 + 0; tris[i * 6 + 1] = i * 2 + 2; tris[i * 6 + 2] = i * 2 + 1;
             tris[i * 6 + 3] = i * 2 + 1; tris[i * 6 + 4] = i * 2 + 2; tris[i * 6 + 5] = i * 2 + 3;
        }

        cols = new Color[verts.Length];
		for (int i = 0; i < cols.Length; i++)
		{
			cols[i] = Color.white;
		}
    }

    void UpdateBounds()
    {
        fieldBounds = new Rect(
            -width / 2.0f, 
            -length / 2.0f, 
            width, 
            length
        );

        // Belongs to Team1
        areaTeam1Bounds = new Rect(
            -(width * areaWidthPerc) / 2.0f, 
            -(length / 2.0f), 
            width * areaWidthPerc, 
            length * areaLengthPerc);

        // Belongs to Team2
        areaTeam2Bounds = new Rect(
            -(width * areaWidthPerc) / 2.0f, 
            (length / 2.0f) - (length * areaLengthPerc), 
            width * areaWidthPerc, 
            length * areaLengthPerc);
    }

}
