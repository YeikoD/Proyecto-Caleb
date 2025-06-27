using UnityEngine;
using static XNode.Node;
using XNode;

[CreateNodeMenu("IA/Acciones/MoverA")]
public class NodoMoverA : NodoAccionIA
{
	public string idDestino; // ID para buscar el punto con Referencias

	private bool inicioMovimiento = false;

	public override NodoIA Ejecutar(NPC npc)
	{
		if (!inicioMovimiento)
		{
			// Obtener el Transform destino desde el banco de referencias
			var destino = Referencias.Instancia.Obtener(idDestino);
			if (destino == null)
			{
				Debug.LogWarning($"[{npc.Nombre}] NodoMoverA: destino '{idDestino}' no encontrado.");
				return GetSalida(); // Salto siguiente por si no existe destino
			}

			npc.Controlador.MoverA(destino);
			inicioMovimiento = true;
			Debug.Log($"[{npc.Nombre}] Iniciando movimiento a {idDestino}");
		}

		// Mientras no llegó, sigo en este nodo
		if (!npc.Controlador.moverNpc.HaLlegado())
			return this;

		// Reset para la próxima vez que se use el nodo
		inicioMovimiento = false;

		Debug.Log($"[{npc.Nombre}] Llegó a {idDestino}");
		return GetSalida();
	}
}
