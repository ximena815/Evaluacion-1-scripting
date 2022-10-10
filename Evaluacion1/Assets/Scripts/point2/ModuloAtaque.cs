using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class ModuloAtaque : MonoBehaviour
{
    [SerializeField] int Rango;
    [SerializeField] int costo;
    [SerializeField] TileMap MapaTiel;
    Unit unidad;

    private bool Arriba = false;
    private bool Abajo = false;
    private bool Izquierda = false;
    private bool Derecha = false;

    public bool arriba {set => Arriba = value; }
    public bool izquierda { set => Izquierda = value; }
    public bool derecha { set => Derecha = value; }
    public bool abajo {set => Abajo = value; }


    private void Start()
    {
        unidad = gameObject.GetComponent<Unit>();
    }

    private void Update()
    {
        if (Arriba)
        {
            for(int y = 1; y <= Rango; y++)
            {
                MapaTiel.ClBoxCollider[unidad.tileX,unidad.tileY + y].ColliderArriba();
            }
            Arriba = false;
            unidad.MovimientoRestante -= costo;
        }
        if (Izquierda)
        {
            for (int x = 1; x <= Rango; x++)
            {
                MapaTiel.ClBoxCollider[unidad.tileX - x, unidad.tileY].ColliderArriba();
            }
            izquierda = false;
            unidad.MovimientoRestante -= costo;
        }
        if (Derecha)
        {
            for (int x = 1; x <= Rango; x++)
            {
                MapaTiel.ClBoxCollider[unidad.tileX + x, unidad.tileY].ColliderArriba();
            }
            Derecha = false;
            unidad.MovimientoRestante -= costo;
        }
        if (Abajo)
        {
            for (int y = 1; y <= Rango; y++)
            {
                MapaTiel.ClBoxCollider[unidad.tileX, unidad.tileY - y].ColliderArriba();
            }
            Abajo = false;
            unidad.MovimientoRestante -= costo;
        }
      
    }
}
