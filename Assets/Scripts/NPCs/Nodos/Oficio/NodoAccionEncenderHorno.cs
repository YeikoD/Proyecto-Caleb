using UnityEngine;

[CreateNodeMenu("IA/Acciones/Encender Horno")]
[NodeTint("#FFD580")] // Un color anaranjado para que se note
public class NodoAccionEncenderHorno : NodoAccionIA
{
	public string idHorno = "horno";
	public string idItemLe�a = "madera";

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
			Debug.LogWarning($"[{npc.Nombre}] No se encontr� el horno con ID '{idHorno}'.");
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
			Debug.Log($"[{npc.Nombre}] El horno ya est� encendido.");
			return GetSalida();
		}

		var itemLe�a = ItemDB.Instancia?.ObtenerPorID(idItemLe�a);
		if (itemLe�a == null)
		{
			Debug.LogWarning($"[{npc.Nombre}] No se encontr� el �tem '{idItemLe�a}' en ItemDB.");
			return this;
		}

		if (npc.Inventario.ObtenerCantidad(itemLe�a) < 1)
		{
			Debug.Log($"[{npc.Nombre}] No tiene le�a para encender el horno.");
			return this;
		}

		bool quitado = npc.Inventario.QuitarItem(itemLe�a, 1);
		if (!quitado)
		{
			Debug.LogWarning($"[{npc.Nombre}] Fall� al quitar le�a del inventario.");
			return this;
		}

		horno.Encender();
		Debug.Log($"[{npc.Nombre}] Encendi� el horno '{idHorno}'.");
		return GetSalida();
	}

	public override string GetDescripcion() => $"Encender '{idHorno}' usando '{idItemLe�a}'";
}
