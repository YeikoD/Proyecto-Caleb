using UnityEngine;

[CreateAssetMenu(menuName = "Dialogo/Efectos/Cambiar Relacion")]
public class CambiarRelacion : EfectoDialogo
{
	public int cantidad;

	public override void EjecutarEfecto(GameObject npc, GameObject jugador)
	{
		Debug.Log($"Relaci�n modificada en {cantidad} puntos con {npc.name}");
		// L�gica real: npc.GetComponent<NPCSocial>()?.ModificarRelacion(jugador, cantidad);
	}
}
