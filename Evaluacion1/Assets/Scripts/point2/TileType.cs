using UnityEngine;
using System.Collections;

[System.Serializable]
public class TileType {

	public string nombre;
	public GameObject tileVisualPrefab;

	public bool isWalkable = true;
	public float costoMovimiento = 1;

}
