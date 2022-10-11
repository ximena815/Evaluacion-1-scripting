using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Events;

public class ControladorEvento : MonoBehaviour
{
    [Header("Personajes")]
    [SerializeField] Unit Heroe;
    [SerializeField] Unit Soldado1;
    [SerializeField] Unit Soldado2;

    [Header("UI")]
    [SerializeField] RectTransform JugadorUI;
    [SerializeField] Text contador;

    [Header("Propiedades")]
    [SerializeField] float TiempoJugador;
    [SerializeField] float TiempoEnemigo;
    private float TiempoActual = 0;
    private bool TurnodelJugador = true;
    private bool trigger = true;

    [Header("Eventos de Unity")]
    [SerializeField] UnityEvent OpJugadorTurno;
    [SerializeField] UnityEvent OpEnemigoTurno;

    private void Update()
    {
        if (TurnodelJugador)
        {
            if (trigger)
            {
                OpJugadorTurno.Invoke();
                TiempoActual = TiempoJugador;
                trigger = false;
            }
            TiempoActual -= Time.deltaTime;
            contador.text = Mathf.CeilToInt(TiempoActual).ToString() + " : Turno del Heroe";
            if(TiempoActual < 0)
            {
                TurnodelJugador = false;
                JugadorUI.DOMoveX(238f + 300f, 0.5f);
                trigger = true;
            }
        }
        if (!TurnodelJugador)
        {
            if (trigger)
            {
                OpEnemigoTurno.Invoke();
                TiempoActual = TiempoEnemigo;
                trigger = false;
            }
            TiempoActual -= Time.deltaTime;
            contador.text = Mathf.CeilToInt(TiempoActual).ToString() + " : turno del enemigo";
            if (TiempoActual < 0)
            {
                TurnodelJugador = true;
                JugadorUI.DOMoveX(238f - 132, 0.5f);
                TurnoJugadorDenuevo();
                trigger = true;
            }
        }
    }

    private void TurnoJugadorDenuevo()
    {
        Heroe.turnoSiguientee();
        Soldado1.turnoSiguientee();
        Soldado2.turnoSiguientee();
    }
}
