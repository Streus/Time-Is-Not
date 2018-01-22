using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

[ExecuteInEditMode]
public class Atlas : MonoBehaviour
{
	#region INSTANCE_VARS

	[SerializeField]
	private GameObject maxpoint;
	[SerializeField]
	private int resolution;
	[SerializeField]
	private bool bindToWidth;
	[SerializeField]
	private LayerMask mask;

	[SerializeField]
	private bool diagonalRoutes = false;

	public Color graphColor = Color.green;
	public bool gizmoGrid = true;
	public bool gizmoNodes = true;

	[SerializeField]
	private Dictionary<Vector2, Node> graph; //TODO change data structure

	private float cellDimension
	{
		get
		{
			if (bindToWidth)
				return (maxpoint.transform.position.x - transform.position.x) / resolution;
			else
				return (maxpoint.transform.position.y - transform.position.y) / resolution;
		}
	}
	#endregion

	#region INSTANCE_METHODS
	public void Awake()
	{
		if (graph == null)
			graph = new Dictionary<Vector2, Node> ();
	}

	public void Update()
	{

	}

	/// <summary>
	/// Generate a graph based on the bounds and resolution defined in this Atlas.
	/// </summary>
	public void generate()
	{
		if (maxpoint.transform.position.x < transform.position.x
		   || maxpoint.transform.position.y < transform.position.y)
		{
			Debug.LogError ("Invalid Maxpoint placement! Maxpoint should have a greater" +
				" (x, y) than the Atlas.");
			return;
		}

		//reset the graph to a blank collection
		if (graph == null)
			graph = new Dictionary<Vector2, Node> ();
		else
			graph.Clear();

		Profiler.BeginSample ("Atlas Generate", this);

		//staring position for node placement as well as size of square cells
		float cd = cellDimension;
		Vector2 nodePlacePos = new Vector2 (transform.position.x + (cd / 2), transform.position.y + (cd / 2));
		Vector2 nodeDimensions = new Vector2 (cd, cd);

		//place nodes and make connections between nodes
		for (int i = 0; i < resolution; i++)
		{
			for (int j = 0; j < resolution; j++)
			{
				RaycastHit2D hit;
				hit = Physics2D.BoxCast (nodePlacePos, nodeDimensions, 0f, Vector2.zero, 0f, mask);
				if (hit.collider == null)
				{
					//place node
					Node curr = new Node (nodePlacePos);
					graph.Add (nodePlacePos, curr);

					//build connections
					Vector2 left = new Vector2(nodePlacePos.x - nodeDimensions.x, nodePlacePos.y);
					Vector2 down = new Vector2(nodePlacePos.x, nodePlacePos.y - nodeDimensions.y);

					Node lNode, dNode;
					if (graph.TryGetValue (left, out lNode))
					{
						curr.links.Add (lNode);
						lNode.links.Add (curr);
					}
					if (graph.TryGetValue (down, out dNode))
					{
						curr.links.Add (dNode);
						dNode.links.Add (curr);
					}

					if (diagonalRoutes)
					{
						Vector2 downLeft = nodePlacePos - nodeDimensions;
						Vector2 downRight = new Vector2 (nodePlacePos.x + nodeDimensions.x, nodePlacePos.y - nodeDimensions.y);

						Node dlNode, drNode;
						if (graph.TryGetValue (downLeft, out dlNode))
						{
							curr.links.Add (dlNode);
							dlNode.links.Add (curr);
						}
						if (graph.TryGetValue (downRight, out drNode))
						{
							curr.links.Add (drNode);
							drNode.links.Add (curr);
						}
					}
				}

				//move right one cell
				nodePlacePos = new Vector2 (nodePlacePos.x + cellDimension, nodePlacePos.y);
			}

			//move up a row
			nodePlacePos = new Vector2 (transform.position.x + (cd / 2), nodePlacePos.y + cellDimension);
		}

		Profiler.EndSample ();

		Debug.Log ("Generation Successful!");
	}

	/// <summary>
	/// Clear the current graph held in this Atlas.
	/// </summary>
	public void clear()
	{
		graph.Clear ();
		Debug.Log ("Graph Cleared!");
	}

	#region GETTERS_SETTERS
	public GameObject getMaxpoint()
	{
		return maxpoint;
	}
	public void setMaxpoint(GameObject obj)
	{
		maxpoint = obj;
	}

	public int getResolution()
	{
		return resolution;
	}
	public void setResolution(int res)
	{
		resolution = res;
	}

	public bool getBindToWidth()
	{
		return bindToWidth;
	}
	public void setBindToWidth(bool btw)
	{
		bindToWidth = btw;
	}

	public LayerMask getMask()
	{
		return mask;
	}
	public void setMask(LayerMask mask)
	{
		this.mask = mask;
	}

	public bool getDiagonalRoutes()
	{
		return diagonalRoutes;
	}
	public void setDiagonalRoutes(bool dr)
	{
		diagonalRoutes = dr;
	}
	#endregion

	public void OnDrawGizmos()
	{
		if (!gizmoGrid || maxpoint == null)
			return;

		Gizmos.color = gizmoNodes ? Color.grey : graphColor;
		Vector3 start, end;

		if (maxpoint.transform.position.x < transform.position.x ||
		    maxpoint.transform.position.y < transform.position.y)
			return;

		//horizontal lines
		float height = maxpoint.transform.position.y - transform.position.y;
		for (float i = 0; i <= height; i += cellDimension)
		{
			start = new Vector3 (transform.position.x, transform.position.y + i);
			end = new Vector3 (maxpoint.transform.position.x, transform.position.y + i);
			Gizmos.DrawLine (start, end);
		}

		//vertical lines
		float width = maxpoint.transform.position.x - transform.position.x;
		for (float j = 0; j <= width; j += cellDimension)
		{
			start = new Vector3 (transform.position.x + j, transform.position.y);
			end = new Vector3 (transform.position.x + j, maxpoint.transform.position.y);
			Gizmos.DrawLine (start, end);
		}

		if (!gizmoNodes || graph == null || graph.Values.Count <= 0)
			return;

		Gizmos.color = graphColor;

		//draw graph
		foreach (Node n in graph.Values)
		{
			//node
			Gizmos.DrawWireSphere ((Vector3)n.getPosition(), cellDimension/4);

			//node's connections
			for (int i = 0; i < n.links.Count; i++)
			{
				Gizmos.DrawLine ((Vector3)n.getPosition (), (Vector3)n.links [i].getPosition ());
			}
		}
	}
	#endregion

	#region INTERNAL_TYPES

	[System.Serializable]
	public class Node
	{
		[SerializeField]
		private Vector2 position;
		public List<Node> links;

		public Node(Vector2 position)
		{
			this.position = position;
			links = new List<Node>();
		}

		public Vector2 getPosition()
		{
			return position;
		}
	}

	[System.Serializable]
	public class GraphMap
	{
		private Node[,] graph;
		private Vector2 min, max;
		private float cellSize;

		public GraphMap(Vector2 min, Vector2 max, float cellSize, int width, int height)
		{
			this.min = min;
			this.max = max;
			this.cellSize = cellSize;
			graph = new Node[width, height];
		}

		public void put(Vector2 pos, Node n)
		{
			Vector2 p = normalize (pos);
			if (p == null)
				return;
			graph [(int)p.x, (int)p.y] = n;
		}

		public bool get(Vector2 pos, out Node n)
		{
			n = null;
			Vector2 p = normalize (pos);
			if (p == null)
				return false;

			n = graph [(int)p.x, (int)p.y];
			return true;
		}

		public Node this[int x, int y]
		{
			get
			{
				return graph [x, y];
			}
		}

		/// <summary>
		/// Normalize the specified vector to this map's grid
		/// </summary>
		private Vector2 normalize(Vector2 i)
		{
			if (i.x > max.x)
				i = new Vector2 (max.x, i.y);
			if (i.y > max.y)
				i = new Vector2 (i.x, max.y);

			Vector2 p = i - min;
			p = new Vector2 (Mathf.Floor(p.x / cellSize), Mathf.Floor(p.y / cellSize));
			return p;
		}
	}
	#endregion
}
