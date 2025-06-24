using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public enum TipoPuesto
{
	Mesa,
	Horno,
}

public class PuestoTrabajo : OficioSystems
{
	private HashSet<Transform> npcsProcesados = new HashSet<Transform>(); // NPCs ya procesados
	public TipoPuesto tipoPuesto; // Tipo de almacén asociado a la mesa de trabajo

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

		switch (tipoPuesto)
		{
			case TipoPuesto.Mesa:
				break;
			case TipoPuesto.Horno:
				if (!hornoEncendido)
				{
					StartCoroutine(DetenerHorno(Random.Range(119, 241))); // Inicia el horno y lo detiene después de 5 segundos
				}
				break;
			default:
				Debug.LogWarning("Tipo de almacén no reconocido: " + tipoPuesto);
				break;
		}

		npcsProcesados.Add(other.transform); // Marca el NPC como procesado
	}

	private IEnumerator DetenerHorno(float tiempoEspera)
	{
		yield return new WaitForSeconds(tiempoEspera);
		hornoEncendido = false; // Desactiva el horno después del tiempo de espera
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