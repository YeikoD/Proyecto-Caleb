using UnityEngine;

[CreateNodeMenu("IA/Acciones/Tomar Item")]
public class NodoAccionTomarItem : NodoAccionIA
{
	public string idAlmacen = "almacen";   // ID en Referencias
	public string idItem = "item";                 // ID del item a tomar
	public int cantidad = 1;                         // Cantidad a tomar

	public override NodoIA Ejecutar(NPC npc)
	{
		var refSistema = Referencias.Instancia;
		if (refSistema == null)
		{
			Debug.LogWarning($"[{npc.Nombre}] NodoAccionTomarItem: Referencias no inicializada.");
			return this; // Espera
		}

		var objAlmacen = refSistema.Obtener(idAlmacen);
		if (objAlmacen == null)
		{
			Debug.LogWarning($"[{npc.Nombre}] NodoAccionTomarItem: No se encontró el objeto con ID '{idAlmacen}'.");
			return this; // Espera
		}

		var inventarioAlmacen = objAlmacen.GetComponent<Inventario>();
		if (inventarioAlmacen == null)
		{
			Debug.LogWarning($"[{npc.Nombre}] NodoAccionTomarItem: El objeto '{idAlmacen}' no tiene componente Inventario.");
			return this; // Espera
		}

		var item = ItemDB.Instancia?.ObtenerPorID(idItem);
		if (item == null)
		{
			Debug.LogWarning($"[{npc.Nombre}] NodoAccionTomarItem: No se encontró ItemData con ID '{idItem}'.");
			return this; // Espera
		}

		// Verificamos que haya suficiente cantidad
		if (inventarioAlmacen.ObtenerCantidad(item) >= cantidad)
		{
			// Quitar del almacen
			bool quitado = inventarioAlmacen.QuitarItem(item, cantidad);
			if (quitado)
			{
				// Agregar al inventario del npc
				if (npc.Inventario == null)
				{
					Debug.LogWarning($"[{npc.Nombre}] NodoAccionTomarItem: NPC no tiene inventario asignado.");
					return this; // Espera
				}

				npc.Inventario.AgregarItem(item, cantidad);
				Debug.Log($"[{npc.Nombre}] NodoAccionTomarItem: Tomó {cantidad}x '{item.nombreItem}' del almacen '{idAlmacen}'.");

				var siguiente = GetSalida();
				if (siguiente == null)
				{
					Debug.Log($"[{npc.Nombre}] NodoAccionTomarItem: No hay salida conectada. Permaneciendo en el nodo.");
					return this;
				}

				return siguiente;
			}
			else
			{
				Debug.LogWarning($"[{npc.Nombre}] NodoAccionTomarItem: Error al quitar item del almacen.");
				return this; // Espera
			}
		}
		else
		{
			Debug.LogWarning($"[{npc.Nombre}] NodoAccionTomarItem: No hay suficiente '{item.nombreItem}' en el almacen.");
			return this; // Espera
		}
	}

	public override string GetDescripcion() => $"Tomar {cantidad}x '{idItem}' de '{idAlmacen}'";
}
