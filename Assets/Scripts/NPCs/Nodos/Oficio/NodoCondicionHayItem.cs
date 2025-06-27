using UnityEngine;

[CreateNodeMenu("IA/Condiciones/Verificar Item")]
public class NodoCondicionHayItem : NodoCondicionIA
{
	public string idAlmacen = "ALMACEN-PANADERIA"; // ID en Referencias
	public string idItem = "item";          // ID del ítem en ItemDB

	public override NodoIA Ejecutar(NPC npc)
	{
		var refSistema = Referencias.Instancia;
		if (refSistema == null)
		{
			Debug.LogWarning($"[{npc.Nombre}] NodoCondicionHayItem: Referencias no inicializada.");
			return GetSalidaFalsa() ?? this;
		}

		var objAlmacen = refSistema.Obtener(idAlmacen);
		if (objAlmacen == null)
		{
			Debug.LogWarning($"[{npc.Nombre}] NodoCondicionHayItem: No se encontró el objeto con ID '{idAlmacen}'.");
			return GetSalidaFalsa() ?? this;
		}

		var inventario = objAlmacen.GetComponent<Inventario>();
		if (inventario == null)
		{
			Debug.LogWarning($"[{npc.Nombre}] NodoCondicionHayItem: El objeto '{idAlmacen}' no tiene componente Inventario.");
			return GetSalidaFalsa() ?? this;
		}

		var item = ItemDB.Instancia?.ObtenerPorID(idItem);
		if (item == null)
		{
			Debug.LogWarning($"[{npc.Nombre}] NodoCondicionHayItem: No se encontró ItemData con ID '{idItem}'.");
			return GetSalidaFalsa() ?? this;
		}

		bool hayItem = inventario.ObtenerCantidad(item) > 0;
		Debug.Log($"[{npc.Nombre}] NodoCondicionHayItem: Hay item = {hayItem}");

		var siguiente = hayItem ? GetSalidaVerdadera() : GetSalidaFalsa();
		return siguiente != null ? siguiente : this;
	}

	public override string GetDescripcion() => $"¿Hay item en '{idAlmacen}'?";
}
