using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HCNullFieldVisualizer : MonoBehaviour
{
	private const float FIELD_FOV = 360f;

	#region INSTANCE_VARS
	[SerializeField]
	private HermitCrab crab;

	[Tooltip("The number of casts per degree of FoV.")]
	[SerializeField]
	private int resolution;

	private MeshFilter mFilter;
	private Mesh mesh;

	#endregion

	#region INSTANCE_METHODS

	public void Awake()
	{
		mFilter = GetComponent<MeshFilter> ();

		//setup mesh filter
		mesh = new Mesh ();
		mesh.name = "SightMesh";
		mesh.MarkDynamic ();
		mFilter.mesh = mesh;

		GetComponent<MeshRenderer> ().sortingLayerName = "Sight";
		GetComponent<MeshRenderer> ().sortingOrder = 1000;
	}

	public void LateUpdate()
	{
		//don't do any of this if we're too far from the player
		if (!crab.inCullingRange ())
			return;

		//build list of points
		int casts = Mathf.RoundToInt (FIELD_FOV * resolution);
		float dAngle = FIELD_FOV / casts;
		List<Vector3> points = new List<Vector3> ();

		for (int i = 0; i <= casts; i++)
		{
			float angle = transform.eulerAngles.z - FIELD_FOV / 2 + dAngle * i;
			Vector3 dir = new Vector3 (Mathf.Sin (-angle * Mathf.Deg2Rad), Mathf.Cos (angle * Mathf.Deg2Rad), 0f);
			RaycastHit2D hit;

			//if there's a hit, add the hit point, else, add a point at max range
			bool saveQHT = Physics2D.queriesHitTriggers;
			Physics2D.queriesHitTriggers = false;
			hit = Physics2D.Raycast (transform.position, dir, crab.getNullFieldRadius(), crab.getObstMask());
			Physics2D.queriesHitTriggers = saveQHT;
			if (hit.collider != null)
				points.Add ((Vector3)hit.point);
			else
				points.Add (transform.position + dir * crab.getNullFieldRadius());
		}

		//create arrays of vertices and triangles to build mesh
		int vCount = points.Count + 1;
		Vector3[] vertices = new Vector3[vCount];
		int[] tris = new int[(vCount - 2) * 3];

		vertices [0] = Vector3.zero;
		for (int i = 0; i < vCount - 1; i++)
		{
			vertices [i + 1] = transform.InverseTransformPoint(points [i]);

			if (i < vCount - 2)
			{
				//gotta add triangle points in reverse order for normals to come out right
				tris [i * 3] = i + 2;
				tris [i * 3 + 1] = i + 1;
				tris [i * 3 + 2] = 0;
			}
		}

		//setup mesh
		mesh.Clear ();
		mesh.vertices = vertices;
		mesh.triangles = tris;
		mesh.RecalculateNormals ();
	}

	public void OnDisable()
	{
		mesh.Clear ();
	}
	#endregion
}
