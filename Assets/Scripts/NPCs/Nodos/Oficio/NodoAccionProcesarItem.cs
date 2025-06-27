using UnityEngine;

[CreateNodeMenu("IA/Acciones/Procesar Item")]
[NodeTint("#FFD580")] // Color pastel de "proceso"
public class NodoAccionProcesarItem : NodoAccionIA
{
	public string idItemEntrada = "harina";
	public int cantidadEntrada = 1;

	public string idItemSalida = "pan";
	public int cantidadSalida = 1;

	public override NodoIA Ejecutar(NPC npc)
	{
		if (npc.Inventario == null)
		{
			Debug.LogWarning($"[{npc.Nombre}] NodoAccionProcesarItem: Inventario no asignado.");
			return this;
		}

		var itemEntrada = ItemDB.Instancia?.ObtenerPorID(idItemEntrada);
		var itemSalida = ItemDB.Instancia?.ObtenerPorID(idItemSalida);

		if (itemEntrada == null || itemSalida == null)
		{
			Debug.LogWarning($"[{npc.Nombre}] NodoAccionProcesarItem: Item no encontrado en DB.");
			return this;
		}

		if (npc.Inventario.ObtenerCantidad(itemEntrada) < cantidadEntrada)
		{
			Debug.Log($"[{npc.Nombre}] No tiene suficiente '{idItemEntrada}' para procesar.");
			return this; // espera a tener lo necesario
		}

		// Proceso
		bool quitado = npc.Inventario.QuitarItem(itemEntrada, cantidadEntrada);
		if (!quitado)
		{
			Debug.LogWarning($"[{npc.Nombre}] Falló al quitar '{idItemEntrada}' del inventario.");
			return this;
		}

		npc.Inventario.AgregarItem(itemSalida, cantidadSalida);
		Debug.Log($"[{npc.Nombre}] Procesó {cantidadEntrada}x '{idItemEntrada}' en {cantidadSalida}x '{idItemSalida}'.");

		return GetSalida();
	}

	public override string GetDescripcion() => $"Convertir {cantidadEntrada}x '{idItemEntrada}' → {cantidadSalida}x '{idItemSalida}'";
}
