using UnityEngine;

public abstract class EfectoDialogo : ScriptableObject
{
	public abstract void EjecutarEfecto(GameObject npc, GameObject jugador);
}
