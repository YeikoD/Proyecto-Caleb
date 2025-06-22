using UnityEngine;

[CreateAssetMenu(menuName = "Dialogo/Efectos/Cambiar Relacion")]
public class CambiarRelacion : EfectoDialogo
{
	public int cantidad;

	public override void EjecutarEfecto(GameObject npc, GameObject jugador)
	{
		Debug.Log($"Relación modificada en {cantidad} puntos con {npc.name}");
		// Lógica real: npc.GetComponent<NPCSocial>()?.ModificarRelacion(jugador, cantidad);
	}
}
