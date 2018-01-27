using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

[ExecuteInEditMode]
public class Atlas : MonoBehaviour
{
	#region STATIC_VARS

	private static Atlas _instance;
	public static Atlas instance { get { return _instance; } }
	#endregion

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
	private GraphMap graph;

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
		if (_instance == null)
			_instance = this;
		else
		{
			Debug.LogError ("More than one Atlas in a scene is not supported!");
			Destroy (gameObject);
		}
	}

	public void Update()
	{

	}

	/// <summary>
	/// Attempts to define the shortest path from start to end using a basic A*-based
	/// algorithm. Returns false if a path could not be found.
	/// </summary>
	public bool findPath(Vector3 start, Vector3 end, out Queue<Vector3> path)
	{
		path = null;

		if (graph == null)
			return false;

		List<Node> closedList = new List<Node> ();
		Queue<Node> openList = new Queue<Node> ();

		Dictionary<Node, float> gScores, fScores;
		gScores = new Dictionary<Node, float> ();
		fScores = new Dictionary<Node, float> ();

		Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node> ();

		Node startNode;
		Node endNode;
		if (graph.get (start, out startNode))
			openList.Enqueue (startNode);
		else
			return false;
		
		if (!graph.get (end, out endNode))
			return false;

		gScores.Add (startNode, 0);
		fScores.Add (startNode, heuristicCost (startNode.getPosition (), end));

		Node current = null;
		while (openList.Count > 0)
		{
			current = openList.Dequeue ();

			if (current.getPosition () == endNode.getPosition ())
			{
				path = new Queue<Vector3> ();
				path.Enqueue ((Vector3)current.getPosition ());
				Node prev;
				while (cameFrom.TryGetValue (current, out prev))
				{
					path.Enqueue ((Vector3)prev.getPosition());
					current = prev;
				}
				return true;
			}

			closedList.Add (current);

			for (int i = 0; i < current.links.Count; i++)
			{
				Node neighbor;
				if (graph.get (current.links [i], out neighbor))
				{
					if (closedList.Contains (neighbor))
						continue;

					if (!openList.Contains (neighbor))
						openList.Enqueue (neighbor);

					float curr_g;
					if (!gScores.TryGetValue (current, out curr_g))
						curr_g = float.PositiveInfinity;

					float neig_g;
					if (!gScores.TryGetValue (neighbor, out neig_g))
						neig_g = float.PositiveInfinity;

					float tent_g = curr_g + Vector2.Distance (current.getPosition (), current.links [i]);
					if (tent_g >= neig_g)
						continue;

					cameFrom.Remove (neighbor);
					cameFrom.Add (neighbor, current);

					gScores.Remove (neighbor);
					gScores.Add (neighbor, tent_g);

					fScores.Remove (neighbor);
					fScores.Add (neighbor, tent_g + heuristicCost (neighbor.getPosition (), endNode.getPosition ()));
				}
			}
		}
		return false;
	}

	private float heuristicCost(Vector3 start, Vector3 end)
	{
		return (Mathf.Abs (end.x - start.x) + Mathf.Abs (end.y - start.y)) * cellDimension;
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
		float cd = cellDimension;
		int width = Mathf.FloorToInt ((maxpoint.transform.position.x - transform.position.x) / cd);
		int height = Mathf.FloorToInt ((maxpoint.transform.position.y - transform.position.y) / cd);
		graph = new GraphMap (transform.position, maxpoint.transform.position, cd, width, height);
		Debug.Log (graph.ToString ()); //DEBUG graph toString

		Profiler.BeginSample ("Atlas Generate", this);

		//staring position for node placement as well as size of square cells
		Vector2 nodePlacePos = new Vector2 (transform.position.x + (cd / 2), transform.position.y + (cd / 2));
		Vector2 nodeDimensions = new Vector2 (cd, cd);

		//place nodes and make connections between nodes
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				RaycastHit2D hit;
				hit = Physics2D.BoxCast (nodePlacePos, nodeDimensions, 0f, Vector2.zero, 0f, mask);
				if (hit.collider == null)
				{
					//place node
					Node curr = new Node (nodePlacePos);
					graph.put (nodePlacePos, curr);

					//build connections
					Vector2 left = new Vector2 (nodePlacePos.x - cd, nodePlacePos.y);
					Vector2 down = new Vector2 (nodePlacePos.x, nodePlacePos.y - cd);

					Node lNode, dNode;
					if (graph.get (left, out lNode))
					{
						curr.links.Add (lNode.getPosition ());
						lNode.links.Add (curr.getPosition ());
					}
					if (graph.get (down, out dNode))
					{
						curr.links.Add (dNode.getPosition ());
						dNode.links.Add (curr.getPosition ());
					}

					if (diagonalRoutes)
					{
						Vector2 downLeft = nodePlacePos - nodeDimensions;
						Vector2 downRight = new Vector2 (nodePlacePos.x + cd, nodePlacePos.y - cd);

						Node dlNode, drNode;
						if (graph.get (downLeft, out dlNode))
						{
							curr.links.Add (dlNode.getPosition ());
							dlNode.links.Add (curr.getPosition ());
						}
						if (graph.get (downRight, out drNode))
						{
							curr.links.Add (drNode.getPosition ());
							drNode.links.Add (curr.getPosition ());
						}
					}
				}

				//move right one cell
				nodePlacePos = new Vector2 (nodePlacePos.x + cd, nodePlacePos.y);
			}

			//move up a row
			nodePlacePos = new Vector2 (transform.position.x + (cd / 2), nodePlacePos.y + cd);
		}

		Profiler.EndSample ();

		Debug.Log ("Generation Successful!");
	}

	/// <summary>
	/// Clear the current graph held in this Atlas.
	/// </summary>
	public void clear()
	{
		graph.clear ();
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

		if (!gizmoNodes || graph == null || graph.size <= 0)
			return;

		Gizmos.color = graphColor;

		//draw graph
		for (int i = 0; i < graph.capacity; i++)
		{
			if (graph [i] == null|| graph [i].links == null || graph[i].links.Count <= 0)
				continue;

			//node
			Gizmos.DrawWireSphere ((Vector3)graph[i].getPosition(), cellDimension/4);

			//node's connections
			for (int j = 0; j < graph[i].links.Count; j++)
			{
				Node endPnt;
				if(graph.get(graph[i].links [j], out endPnt))
					Gizmos.DrawLine ((Vector3)graph[i].getPosition (), endPnt.getPosition ());
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
		public List<Vector2> links;

		public Node(Vector2 position)
		{
			this.position = position;
			links = new List<Vector2>();
		}

		public Vector2 getPosition()
		{
			return position;
		}
	}

	[System.Serializable]
	public class GraphMap
	{
		[SerializeField]
		private Node[] graph;
		[SerializeField]
		private Vector2 min, max;
		[SerializeField]
		private float cellSize;
		[SerializeField]
		private int width, height;

		[SerializeField]
		private int _size;
		public int size { get { return _size; } }

		public int capacity
		{
			get { return graph.Length; }
		}

		public GraphMap() : this(default(Vector2), default(Vector2), 1f, 1, 1) { }

		public GraphMap(Vector2 min, Vector2 max, float cellSize, int width, int height)
		{
			this.min = min;
			this.max = max;
			this.cellSize = cellSize;
			this.width = width;
			this.height = height;
			graph = new Node[width * height];
			_size = 0;
		}

		private int xyti(float x, float y)
		{
			return xyti ((int)x, (int)y);
		}
		private int xyti(int x, int y)
		{
			return (int)(x + y * width);
		}

		public void put(Vector2 pos, Node n)
		{
			Vector2 p = normalize (pos);
			try
			{
				graph [xyti(p.x, p.y)] = n;
			}
			catch(System.IndexOutOfRangeException ioore)
			{
				Debug.Log (ioore.ToString() + "\n" + p.ToString() + " -> " + xyti(p.x, p.y));
			}
			_size++;
		}

		public bool get(Vector2 pos, out Node n)
		{
			n = null;

			if (pos.x > max.x || pos.y > max.y)
				return false;
			if (pos.x < min.x || pos.y < min.y)
				return false;

			Vector2 p = normalize (pos);

			try
			{
				n = graph [xyti(p.x, p.y)];
			}
			catch(System.IndexOutOfRangeException ioore)
			{
				return false;
			}

			if (n == null)
				return false;
			return true;
		}

		public Node this[int i]
		{
			get
			{
				return graph [i];
			}
		}

		public Node this[int x, int y]
		{
			get
			{
				return graph [xyti(x, y)];
			}
		}

		public void clear()
		{
			if (graph == null)
				return;
			graph = new Node[width * height];
		}

		/// <summary>
		/// Normalize the specified vector to this map's grid
		/// </summary>
		private Vector2 normalize(Vector2 i)
		{
			Vector2 p = i - min;
			p = new Vector2 (Mathf.Floor(p.x / cellSize), Mathf.Floor(p.y / cellSize));
			return p;
		}

		public override string ToString ()
		{
			string str = "GraphMap:";
			str += "\nSize: " + _size;
			str += "\nCapacity: " + capacity;
			str += "\nMin: " + min.ToString ();
			str += "\nMax: " + max.ToString ();
			str += "\nCell Size: " + cellSize.ToString ("N");
			str += "\nWidth: " + width;
			str += "\nHeight: " + height;

			return str;
		}
	}
	#endregion
}
