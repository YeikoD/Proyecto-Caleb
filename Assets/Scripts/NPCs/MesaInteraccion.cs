using System.Collections.Generic;
using UnityEngine;

public enum AlmacenType
{
	Mesa, // Almacén general
	AlmacenGeneral, // Almacén general
	AlmacenHerrero, // Almacén del herrero
	AlmacenPanadero // Almacén del panadero
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

		if (!other.CompareTag("NPC")) return;

		// Obtenemos el sistema base del NPC y lo hacemos rotar hacia la mesa
		var npcBase = other.GetComponent<NPCBaseSystems>();
		if (npcBase != null && !mirandoPlayer)
		{
			npcBase.RotateTowardsTarget(transform.position); // El NPC mira hacia la mesa
		}

		switch (almacenType)
		{
			case AlmacenType.Mesa:

				break;
			case AlmacenType.AlmacenGeneral:
				if (other.TryGetComponent<ArtesanosSystems>(out ArtesanosSystems herrero))
				{
					
					if (herrero.sinMadera)
					{
						EventManager.TriggerEvent("InicioTransporte", ("herrero", "madera"));
					}
					else if (herrero.sinHierro)
					{
						EventManager.TriggerEvent("InicioTransporte", ("herrero", "hierro"));
					}
					else
					{
						Debug.LogWarning("El herrero no necesita recursos en este momento.");
					}
				}
				else if (other.TryGetComponent<ArtesanosSystems>(out ArtesanosSystems panadero))
				{

				}
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

	private void OnTriggerExit(Collider other)
	{
		npcsProcesados.Remove(other.transform); // Limpia el registro al salir
	}
}