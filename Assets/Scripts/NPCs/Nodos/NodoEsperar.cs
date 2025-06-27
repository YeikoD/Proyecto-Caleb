using UnityEngine;
using System.Collections.Generic;

[CreateNodeMenu("IA/Acciones/Esperar")]
public class NodoEsperar : NodoAccionIA
{
	public float tiempoEspera = 2f;

	// Diccionario para guardar el tiempo de espera por NPC
	private Dictionary<NPC, float> tiemposInicio = new();

	public override NodoIA Ejecutar(NPC npc)
	{
		if (!tiemposInicio.ContainsKey(npc))
			tiemposInicio[npc] = Time.time;

		float tiempoInicial = tiemposInicio[npc];

		if (Time.time - tiempoInicial < tiempoEspera)
			return this;

		tiemposInicio.Remove(npc); // limpieza
		Debug.Log($"[{npc.Nombre}] Esperó {tiempoEspera} segundos");
		return GetSalida();
	}
}
