using UnityEngine;
using System.Collections.Generic;

public class Node2{

	
	[SerializeField] int costo;

	public List<Node2> Vecinos;
	public int x;
	public int y;

    

    public Node2() {
		Vecinos = new List<Node2>();
	}
	
	public float DistanceTo(Node2 n) {
		if(n == null) {
			Debug.LogError("Error");
		}
		
		return Vector2.Distance(
			new Vector2(x, y),
			new Vector2(n.x, n.y)
			);
	}
	
}
