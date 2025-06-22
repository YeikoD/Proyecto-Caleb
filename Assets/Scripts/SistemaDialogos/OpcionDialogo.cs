using UnityEngine;

[System.Serializable]
public class OpcionDialogo
{
	public string textoJugador;                    // Qu� dice el jugador
	public LineaDialogo siguienteLinea;                // A qu� nodo va
	public bool terminarDialogo = false;            // �Cierra el di�logo?
	public int cambiarRelacion;                 // Por ejemplo: +5 o -10
	public string llaveCondicion;                  // Variable de condici�n opcional (ej. "TieneHerramienta")
	public bool requiredValue;                   // Valor que debe tener esa variable para mostrarse

	public EfectoDialogo[] efectos;
}
