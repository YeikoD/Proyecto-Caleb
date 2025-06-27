using UnityEngine;

[CreateNodeMenu("IA/Condiciones/Pedir Item al Soberano")]
public class NodoCondicionPedirItem : NodoCondicionIA
{
	public string idAlmacen = "ALMACEN-PRINCIPAL";  // ID del almacén del soberano
	public string idItem = "madera";                // ID del ítem a pedir
	public string claveMemoria = "puedePedirMadera"; // Clave que guarda si el NPC ya sabe que no puede pedir

	public override NodoIA Ejecutar(NPC npc)
	{
		// Si ya sabe que no puede pedir, no insiste para no spamear
		if (npc.Memoria.TryGetValue(claveMemoria, out bool puedePedir) && !puedePedir)
		{
			Debug.Log($"[{npc.Nombre}] Ya sabe que no puede pedir '{idItem}', no insiste.");
			return GetSalidaFalsa();
		}

		var refSistema = Referencias.Instancia;
		if (refSistema == null)
		{
			Debug.LogWarning($"[{npc.Nombre}] Referencias no inicializada.");
			return this; // espera un toque, que no se mate
		}

		var objAlmacen = refSistema.Obtener(idAlmacen);
		if (objAlmacen == null)
		{
			Debug.LogWarning($"[{npc.Nombre}] No se encontró el almacén '{idAlmacen}'.");
			return this; // esperar a que exista
		}

		var inventario = objAlmacen.GetComponent<Inventario>();
		if (inventario == null)
		{
			Debug.LogWarning($"[{npc.Nombre}] El almacén '{idAlmacen}' no tiene componente Inventario.");
			return this;
		}

		var item = ItemDB.Instancia?.ObtenerPorID(idItem);
		if (item == null)
		{
			Debug.LogWarning($"[{npc.Nombre}] Item '{idItem}' no encontrado en la base de datos.");
			return this;
		}

		bool hayItem = inventario.ObtenerCantidad(item) > 0;
		bool permisoDelSoberano = VoluntadDelSoberano.Instancia?.PermiteRetiro(idItem) ?? false;

		if (hayItem && permisoDelSoberano)
		{
			Debug.Log($"[{npc.Nombre}] Puede pedir '{idItem}' al soberano.");
			npc.Memoria[claveMemoria] = true;
			return GetSalidaVerdadera();
		}
		else
		{
			Debug.Log($"[{npc.Nombre}] No puede pedir '{idItem}': {(hayItem ? "sin permiso del soberano" : "sin stock")}.");
			npc.Memoria[claveMemoria] = false; // memoriza para no volver a pedir hasta que cambie
			return GetSalidaFalsa();
		}
	}

	public override string GetDescripcion() => $"¿Puede pedir '{idItem}' al soberano?";
}
