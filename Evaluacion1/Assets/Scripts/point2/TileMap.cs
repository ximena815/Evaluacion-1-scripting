using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TileMap : MonoBehaviour
{

	[Header("Characters")]
	[SerializeField] GameObject Hero;
	[SerializeField] GameObject soldier1;
	[SerializeField] GameObject soldier2;

	[Header("General")]
	public GameObject selectedUnit;

	public TileType[] tileTypes;

	int[,] tiles;
	Node2[,] graph;
	public ClickableTile[,] ClBoxCollider;

	[SerializeField] int mapSizeX = 10;
	[SerializeField] int mapSizeY = 10;

	[SerializeField] PosicionBiome[] Water;
	[SerializeField] PosicionBiome[] Sand;
	[SerializeField] PosicionBiome[] Mountain;

	void Start()
	{
		// Setup the selectedUnit's variable
		selectedUnit.GetComponent<Unit>().tileX = (int)selectedUnit.transform.position.x;
		selectedUnit.GetComponent<Unit>().tileY = (int)selectedUnit.transform.position.y;
		selectedUnit.GetComponent<Unit>().mapa = this;

		GenerateMapData();
		GeneratePathfindingGraph();
		GenerateMapVisual();
	}

	void GenerateMapData()
	{
		// Allocate our Mapa tiles
		tiles = new int[mapSizeX, mapSizeY];

		int x, y;

		// Initialize our Mapa tiles to be grass
		for (x = 0; x < mapSizeX; x++)
		{
			for (y = 0; y < mapSizeX; y++)
			{
				tiles[x, y] = 0;
			}
		}

		for (int i = 0; i < Water.Length; i++)
		{
			tiles[Water[i].posicionX, Water[i].posicionY] = 3;
		}
		for (int i = 0; i < Sand.Length; i++)
		{
			tiles[Sand[i].posicionX, Sand[i].posicionY] = 1;
		}
		for (int i = 0; i < Mountain.Length; i++)
		{
			tiles[Mountain[i].posicionX, Mountain[i].posicionY] = 2;
		}

	}

	public float CostToEnterTile(int sourceX, int sourceY, int targetX, int targetY)
	{

		TileType tt = tileTypes[tiles[targetX, targetY]];

		if (UnitCanEnterTile(targetX, targetY) == false)
			return Mathf.Infinity;

		float cost = tt.costoMovimiento;

		if (sourceX != targetX && sourceY != targetY)
		{
			// We are moving diagonally!  Fudge the costo for tie-breaking
			// Purely a cosmetic thing!
			cost += 0.001f;
		}

		return cost;

	}

	void GeneratePathfindingGraph()
	{
		// Initialize the array
		graph = new Node2[mapSizeX, mapSizeY];

		// Initialize a Node for each spot in the array
		for (int x = 0; x < mapSizeX; x++)
		{
			for (int y = 0; y < mapSizeX; y++)
			{
				graph[x, y] = new Node2();
				graph[x, y].x = x;
				graph[x, y].y = y;
			}
		}

		// Now that all the nodes exist, calculate their Vecinos
		for (int x = 0; x < mapSizeX; x++)
		{
			for (int y = 0; y < mapSizeX; y++)
			{

				// This is the 4-way connection version:
				if (x > 0)
					graph[x, y].Vecinos.Add(graph[x - 1, y]);
				if (x < mapSizeX - 1)
					graph[x, y].Vecinos.Add(graph[x + 1, y]);
				if (y > 0)
					graph[x, y].Vecinos.Add(graph[x, y - 1]);
				if (y < mapSizeY - 1)
					graph[x, y].Vecinos.Add(graph[x, y + 1]);


				// This is the 8-way connection version (allows diagonal movement)
				// Try Izquierda
				/*
				if(x > 0) {
					graph[x,y].Vecinos.Add( graph[x-1, y] );
					if(y > 0)
						graph[x,y].Vecinos.Add( graph[x-1, y-1] );
					if(y < mapSizeY-1)
						graph[x,y].Vecinos.Add( graph[x-1, y+1] );
				}

				// Try derecha
				if(x < mapSizeX-1) {
					graph[x,y].Vecinos.Add( graph[x+1, y] );
					if(y > 0)
						graph[x,y].Vecinos.Add( graph[x+1, y-1] );
					if(y < mapSizeY-1)
						graph[x,y].Vecinos.Add( graph[x+1, y+1] );
				}

				// Try straight Arriba and Abajo
				if(y > 0)
					graph[x,y].Vecinos.Add( graph[x, y-1] );
				if(y < mapSizeY-1)
					graph[x,y].Vecinos.Add( graph[x, y+1] );*/

				// This also works with 6-way hexes and n-way variable areas (like EU4)
			}
		}
	}

	void GenerateMapVisual()
	{
		ClBoxCollider = new ClickableTile[mapSizeX,mapSizeY];
		for (int x = 0; x < mapSizeX; x++)
		{
			for (int y = 0; y < mapSizeX; y++)
			{
				TileType tt = tileTypes[tiles[x, y]];
				GameObject go = (GameObject)Instantiate(tt.tileVisualPrefab, new Vector3(x, y, 0), Quaternion.identity);
				ClBoxCollider[x, y] = go.GetComponent<ClickableTile>();
				ClickableTile ct = go.GetComponent<ClickableTile>();
				ct.Xtile = x;
				ct.Ytile = y;
				ct.Mapa = this;
			}
		}
	}

	public Vector3 TileCoordToWorldCoord(int x, int y)
	{
		return new Vector3(x, y, 0);
	}

	public bool UnitCanEnterTile(int x, int y)
	{

		// We could test the unidad's walk/hover/fly type against various
		// terrain flags here to see if they are allowed to enter the tile.

		return tileTypes[tiles[x, y]].isWalkable;
	}

	public void GeneratePathTo(int x, int y)
	{
		// Clear out our unidad's old path.
		selectedUnit.GetComponent<Unit>().TrayectoriaDeCorriente = null;

		if (UnitCanEnterTile(x, y) == false)
		{
			// We probably clicked on a mountain or something, so just quit out.
			return;
		}

		Dictionary<Node2, float> dist = new Dictionary<Node2, float>();
		Dictionary<Node2, Node2> prev = new Dictionary<Node2, Node2>();

		// Setup the "Q" -- the list of nodes we haven't checked yet.
		List<Node2> unvisited = new List<Node2>();

		Node2 source = graph[
							selectedUnit.GetComponent<Unit>().tileX,
							selectedUnit.GetComponent<Unit>().tileY
							];

		Node2 target = graph[
							x,
							y
							];

		dist[source] = 0;
		prev[source] = null;

		// Initialize everything to have INFINITY distance, since
		// we don't know any better Derecha now. Also, it's possible
		// that some nodes CAN'T be reached from the source,
		// which would make INFINITY a reasonable value
		foreach (Node2 v in graph)
		{
			if (v != source)
			{
				dist[v] = Mathf.Infinity;
				prev[v] = null;
			}

			unvisited.Add(v);
		}

		while (unvisited.Count > 0)
		{
			// "u" is going to be the unvisited node with the smallest distance.
			Node2 u = null;

			foreach (Node2 possibleU in unvisited)
			{
				if (u == null || dist[possibleU] < dist[u])
				{
					u = possibleU;
				}
			}

			if (u == target)
			{
				break;  // Exit the while loop!
			}

			unvisited.Remove(u);

			foreach (Node2 v in u.Vecinos)
			{
				//float alt = dist[u] + u.DistanceTo(v);
				float alt = dist[u] + CostToEnterTile(u.x, u.y, v.x, v.y);
				if (alt < dist[v])
				{
					dist[v] = alt;
					prev[v] = u;
				}
			}
		}

		// If we get there, the either we found the shortest route
		// to our target, or there is no route at ALL to our target.

		if (prev[target] == null)
		{
			// No route between our target and the source
			return;
		}

		List<Node2> currentPath = new List<Node2>();

		Node2 curr = target;

		// Step through the "prev" chain and add it to our path
		while (curr != null)
		{
			currentPath.Add(curr);
			curr = prev[curr];
		}

		// derecha now, TrayectoriaDeCorriente describes a route from out target to our source
		// So we need to invert it!

		currentPath.Reverse();

		selectedUnit.GetComponent<Unit>().TrayectoriaDeCorriente = currentPath;
	}


	public void SelectHero()
	{
		selectedUnit = Hero;

	}
	public void SelectSoldier1()
	{
		selectedUnit = soldier1;

	}
	public void SelectSoldier2()
	{
		selectedUnit = soldier2;
	}
}
