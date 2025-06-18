using System.Collections.Generic;
using UnityEngine;

public enum TipoMesaTrabajo
{
	Almacen,
	Horno,
	MesaDeCrafteo,
	Yunque
}

public class MesasArtesanos : MonoBehaviour
{
	private HashSet<Transform> npcsProcesados = new HashSet<Transform>(); // NPCs ya procesados

	private TipoMesaTrabajo tipoMesa;
	public Inventario almacenInv; // Inventario del Almacen asociado a la mesa de trabajo

	private void Start()
	{
			almacenInv = GetComponent<Inventario>(); // Obtener el inventario del almacen asociado a la mesa de trabajo
	}

	private void OnTriggerStay(Collider other)
	{
		if (!other.CompareTag("NPC")) return;

		var ArtesanosSystems = other.GetComponent<ArtesanosSystems>();

		if (ArtesanosSystems != null)
		{
			ArtesanosSystems.RotateTowardsTarget(transform.position); // Hace que el NPC mire la mesa
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("NPC")) return;
		if (npcsProcesados.Contains(other.transform)) return;   // Evita procesar dos veces el mismo NPC

		npcsProcesados.Add(other.transform); // Marca el NPC como procesado
	}

	private void OnTriggerExit(Collider other)
	{
		npcsProcesados.Remove(other.transform); // Limpia el registro al salir
	}
}
