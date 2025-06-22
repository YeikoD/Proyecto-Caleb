using UnityEngine;

[System.Serializable]
public class OpcionDialogo
{
	public string textoJugador;                    // Qué dice el jugador
	public LineaDialogo siguienteLinea;                // A qué nodo va
	public bool terminarDialogo = false;            // ¿Cierra el diálogo?
	public int cambiarRelacion;                 // Por ejemplo: +5 o -10
	public string llaveCondicion;                  // Variable de condición opcional (ej. "TieneHerramienta")
	public bool requiredValue;                   // Valor que debe tener esa variable para mostrarse

	public EfectoDialogo[] efectos;
}
