using UnityEngine;

[CreateNodeMenu("IA/Acciones/Almacenar Item")]
public class NodoAccionAlmacenarItem : NodoAccionIA
{
	public string idAlmacen = "ALMACEN-PANADERIA"; // ID en Referencias
	public string idItem = "pan";                  // ID del ítem a almacenar
	public int cantidad = 1;                       // Cuánto se quiere guardar

	public override NodoIA Ejecutar(NPC npc)
	{
		if (npc.Inventario == null)
		{
			Debug.LogWarning($"[{npc.Nombre}] NodoAccionAlmacenarItem: NPC no tiene inventario.");
			return this; // Espera
		}

		var item = ItemDB.Instancia?.ObtenerPorID(idItem);
		if (item == null)
		{
			Debug.LogWarning($"[{npc.Nombre}] NodoAccionAlmacenarItem: Item '{idItem}' no encontrado.");
			return this; // Espera
		}

		if (npc.Inventario.ObtenerCantidad(item) < cantidad)
		{
			Debug.LogWarning($"[{npc.Nombre}] NodoAccionAlmacenarItem: No tiene suficiente '{item.nombreItem}' para almacenar.");
			return this; // Espera
		}

		var objAlmacen = Referencias.Instancia?.Obtener(idAlmacen);
		if (objAlmacen == null)
		{
			Debug.LogWarning($"[{npc.Nombre}] NodoAccionAlmacenarItem: No se encontró el almacén '{idAlmacen}'.");
			return this; // Espera
		}

		var inventarioAlmacen = objAlmacen.GetComponent<Inventario>();
		if (inventarioAlmacen == null)
		{
			Debug.LogWarning($"[{npc.Nombre}] NodoAccionAlmacenarItem: El objeto '{idAlmacen}' no tiene Inventario.");
			return this; // Espera
		}

		bool quitado = npc.Inventario.QuitarItem(item, cantidad);
		if (!quitado)
		{
			Debug.LogWarning($"[{npc.Nombre}] NodoAccionAlmacenarItem: Error al quitar el ítem del NPC.");
			return this;
		}

		inventarioAlmacen.AgregarItem(item, cantidad);
		Debug.Log($"[{npc.Nombre}] NodoAccionAlmacenarItem: Guardó {cantidad}x '{item.nombreItem}' en '{idAlmacen}'.");
		return GetSalida();
	}

	public override string GetDescripcion() => $"Guardar {cantidad}x '{idItem}' en '{idAlmacen}'";
}
