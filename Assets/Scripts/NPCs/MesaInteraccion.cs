using System.Collections.Generic;
using UnityEngine;

public enum AlmacenType
{
	Mesa,
	AlmacenGeneral,
	AlmacenHerrero,
	AlmacenPanadero
}

public class MesaInteraccion : NPCBaseSystems
{
	private HashSet<Transform> npcsProcesados = new HashSet<Transform>(); // NPCs ya procesados
	public AlmacenType almacenType; // Tipo de almacén asociado a la mesa de trabajo

	public Inventario almacenInv; // Inventario del Almacen asociado a la mesa 

	protected override void Start()
	{
		almacenInv = GetComponent<Inventario>(); // Obtener el inventario del almacen asociado a la mesa de trabajo
	}

	// Usamos la palabra clave 'new' para ocultar el método heredado
	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("NPC")) return;
		if (npcsProcesados.Contains(other.transform)) return;   // Evita procesar dos veces el mismo NPC

		switch (almacenType)
		{
			case AlmacenType.Mesa:
				break;
			case AlmacenType.AlmacenGeneral:
				break;
			case AlmacenType.AlmacenHerrero:
				break;
			case AlmacenType.AlmacenPanadero:
				break;
			default:
				Debug.LogWarning("Tipo de almacén no reconocido: " + almacenType);
				break;
		}

		npcsProcesados.Add(other.transform); // Marca el NPC como procesado
	}

	public void OnTriggerStay(Collider other)
	{
		// Obtenemos el sistema base del NPC y lo hacemos rotar hacia la mesa
		var npcBase = other.GetComponent<NPCBaseSystems>();
		if (npcBase != null )
		{
			npcBase.RotateTowardsTarget(transform.position); // El NPC mira hacia la mesa
		}
	}

	private void OnTriggerExit(Collider other)
	{
		npcsProcesados.Remove(other.transform); // Limpia el registro al salir
	}
}