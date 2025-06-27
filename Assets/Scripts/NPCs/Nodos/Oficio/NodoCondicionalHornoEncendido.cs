using UnityEngine;

[CreateNodeMenu("IA/Condiciones/HornoEncendido")]
public class NodoCondicionHornoEncendido : NodoCondicionIA
{
	public string idHorno = "hornoPanaderia";

	public override NodoIA Ejecutar(NPC npc)
	{
		var hornoTransform = Referencias.Instancia?.Obtener(idHorno);

		if (hornoTransform == null)
		{
			Debug.LogWarning($"[IA] [{npc.Nombre}] NodoCondicionHornoEncendido: horno con ID '{idHorno}' no encontrado. Esperando...");
			return this; // ⚠️ Esperar hasta que el horno esté registrado
		}

		var estadoHorno = hornoTransform.GetComponent<EstadoHorno>();

		if (estadoHorno == null)
		{
			Debug.LogWarning($"[IA] [{npc.Nombre}] NodoCondicionHornoEncendido: el objeto no tiene componente EstadoHorno.");
			return this; // ⚠️ También esperar
		}

		bool hornoEncendido = estadoHorno.EstaEncendido();

		Debug.Log($"[IA] [{npc.Nombre}] NodoCondicionHornoEncendido: horno encendido = {hornoEncendido}");

		return hornoEncendido ? GetSalidaVerdadera() : GetSalidaFalsa();
	}

	public override string GetDescripcion() => $"¿El horno '{idHorno}' está encendido?";
}

