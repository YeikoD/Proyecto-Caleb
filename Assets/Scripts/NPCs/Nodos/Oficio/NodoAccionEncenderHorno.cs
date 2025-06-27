using UnityEngine;

[CreateNodeMenu("IA/Acciones/Encender Horno")]
[NodeTint("#FFD580")] // Un color anaranjado para que se note
public class NodoAccionEncenderHorno : NodoAccionIA
{
	public string idHorno = "horno";
	public string idItemLeña = "madera";

	public override NodoIA Ejecutar(NPC npc)
	{
		if (npc.Inventario == null)
		{
			Debug.LogWarning($"[{npc.Nombre}] No tiene inventario asignado.");
			return this;
		}

		var hornoTransform = Referencias.Instancia?.Obtener(idHorno);
		if (hornoTransform == null)
		{
			Debug.LogWarning($"[{npc.Nombre}] No se encontró el horno con ID '{idHorno}'.");
			return this;
		}

		var horno = hornoTransform.GetComponent<EstadoHorno>();
		if (horno == null)
		{
			Debug.LogWarning($"[{npc.Nombre}] El objeto '{idHorno}' no tiene EstadoHorno.");
			return this;
		}

		if (horno.EstaEncendido())
		{
			Debug.Log($"[{npc.Nombre}] El horno ya está encendido.");
			return GetSalida();
		}

		var itemLeña = ItemDB.Instancia?.ObtenerPorID(idItemLeña);
		if (itemLeña == null)
		{
			Debug.LogWarning($"[{npc.Nombre}] No se encontró el ítem '{idItemLeña}' en ItemDB.");
			return this;
		}

		if (npc.Inventario.ObtenerCantidad(itemLeña) < 1)
		{
			Debug.Log($"[{npc.Nombre}] No tiene leña para encender el horno.");
			return this;
		}

		bool quitado = npc.Inventario.QuitarItem(itemLeña, 1);
		if (!quitado)
		{
			Debug.LogWarning($"[{npc.Nombre}] Falló al quitar leña del inventario.");
			return this;
		}

		horno.Encender();
		Debug.Log($"[{npc.Nombre}] Encendió el horno '{idHorno}'.");
		return GetSalida();
	}

	public override string GetDescripcion() => $"Encender '{idHorno}' usando '{idItemLeña}'";
}
