using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour {

	public int tileX;
	public int tileY;
	public TileMap mapa;
	public List<Node2> TrayectoriaDeCorriente = null;

	int VelocidadMovimiento = 6;
	public float MovimientoRestante=4;

	void Update() {
		if(TrayectoriaDeCorriente != null) {
			int currNode = 0;

			while( currNode < TrayectoriaDeCorriente.Count-1 ) {

				Vector3 start = mapa.TileCoordToWorldCoord( TrayectoriaDeCorriente[currNode].x, TrayectoriaDeCorriente[currNode].y ) + 
					new Vector3(0, 0, -0.5f) ;
				Vector3 end   = mapa.TileCoordToWorldCoord( TrayectoriaDeCorriente[currNode+1].x, TrayectoriaDeCorriente[currNode+1].y )  + 
					new Vector3(0, 0, -0.5f) ;

				Debug.DrawLine(start, end, Color.red);

				currNode++;
			}
		}
		if(Vector3.Distance(transform.position, mapa.TileCoordToWorldCoord( tileX, tileY )) < 0.1f)
			AvanzarRuta();
		transform.position = Vector3.Lerp(transform.position, mapa.TileCoordToWorldCoord( tileX, tileY ), 5f * Time.deltaTime);
	}
	void AvanzarRuta() {
		if(TrayectoriaDeCorriente==null)
			return;

		if(MovimientoRestante <= 0)
			return;

		transform.position = mapa.TileCoordToWorldCoord( tileX, tileY );

		MovimientoRestante -= mapa.CostToEnterTile(TrayectoriaDeCorriente[0].x, TrayectoriaDeCorriente[0].y, TrayectoriaDeCorriente[1].x, TrayectoriaDeCorriente[1].y );
		
		tileX = TrayectoriaDeCorriente[1].x;
		tileY = TrayectoriaDeCorriente[1].y;
		
		TrayectoriaDeCorriente.RemoveAt(0);
		
		if(TrayectoriaDeCorriente.Count == 1) {
			TrayectoriaDeCorriente = null;
		}
	}

	public void turnoSiguientee() {
		while(TrayectoriaDeCorriente!=null && MovimientoRestante > 0) {
			AvanzarRuta();
		}

		MovimientoRestante = VelocidadMovimiento;
	}
}
