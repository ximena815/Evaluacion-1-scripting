using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ControladorAtaque : MonoBehaviour
{
    public int SeleccionarPerso = 0;

    [SerializeField] RectTransform AtaqueUI;
    [SerializeField] Text PA;

    public int PersonajeSelecci {set => SeleccionarPerso = value; }

    [SerializeField] GameObject[] attackModules;


    private void Update()
    {
        PA.text = "PA: " + attackModules[SeleccionarPerso].GetComponent<Unit>().MovimientoRestante;
    }

    public void ActivarAtaqueModuloArriba()
    {
        if(attackModules[SeleccionarPerso].GetComponent<Unit>().MovimientoRestante > 0)
            attackModules[SeleccionarPerso].GetComponent<ModuloAtaque>().arriba = true;
        
    }
    public void ActivarAtaqueModuloIzquierda()
    {
        if (attackModules[SeleccionarPerso].GetComponent<Unit>().MovimientoRestante > 0)
            attackModules[SeleccionarPerso].GetComponent<ModuloAtaque>().izquierda = true;
    }
    public void ActivarAtaqueModuloDerecha()
    {
        if (attackModules[SeleccionarPerso].GetComponent<Unit>().MovimientoRestante > 0)
            attackModules[SeleccionarPerso].GetComponent<ModuloAtaque>().derecha = true;
    }
    public void ActivarAtaqueModuloAbajo()
    {
        if (attackModules[SeleccionarPerso].GetComponent<Unit>().MovimientoRestante > 0)
            attackModules[SeleccionarPerso].GetComponent<ModuloAtaque>().abajo = true;
    }
    
    public void AtaqueMovimientoUIAbajo()
    {
        AtaqueUI.DOMoveY(-200, 0.5f);
    }
    public void AtaqueMovimientoUIArriba()
    {
        AtaqueUI.DOMoveY(166.5f - 92, 0.5f);
    }
}
