using UnityEngine;
using System.Collections.Generic;

[CreateNodeMenu("IA/Acciones/Esperar (Random)")]
public class NodoEsperarRandom : NodoAccionIA
{
	public float minTiempo = 1f;
	public float maxTiempo = 3f;

	// Guarda el tiempo de inicio y duración por NPC
	private Dictionary<NPC, float> tiemposInicio = new();
	private Dictionary<NPC, float> tiemposDuracion = new();

	public override NodoIA Ejecutar(NPC npc)
	{
		// Si el NPC no tiene ya un tiempo asignado, lo generamos
		if (!tiemposInicio.ContainsKey(npc))
		{
			tiemposInicio[npc] = Time.time;
			tiemposDuracion[npc] = Random.Range(minTiempo, maxTiempo);
			Debug.Log($"[{npc.Nombre}] NodoEsperarRandom: Esperando {tiemposDuracion[npc]:0.00} segundos.");
		}

		float inicio = tiemposInicio[npc];
		float duracion = tiemposDuracion[npc];

		if (Time.time - inicio < duracion)
			return this;

		// Se cumplió el tiempo, limpiamos y seguimos
		tiemposInicio.Remove(npc);
		tiemposDuracion.Remove(npc);
		Debug.Log($"[{npc.Nombre}] NodoEsperarRandom: Espera completada.");

		return GetSalida();
	}

	public override string GetDescripcion() => $"Esperar entre {minTiempo} y {maxTiempo} segundos";
}
