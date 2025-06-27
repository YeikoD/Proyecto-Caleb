using UnityEngine;

[CreateNodeMenu("IA/Global/Inicio")]
public class NodoInicio : NodoIA
{
	[Output(backingValue = ShowBackingValue.Never)] public NodoIA salida;

	public override NodoIA Ejecutar(NPC npc)
	{
		Debug.Log($"[{npc.Nombre}] NodoInicio ejecutado.");
		return GetSalida();
	}

	public override string GetDescripcion() => "Nodo inicial de cualquier grafo.";
}
