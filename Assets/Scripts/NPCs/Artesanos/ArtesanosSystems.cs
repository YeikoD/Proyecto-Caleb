using System.Collections;
using UnityEngine;

public enum Oficio
{
	Herrero,
	Panadero
}

public class ArtesanosSystems : MonoBehaviour
{
	private IOficioState estadoActual;

	[Header("Estado del NPC")]
	public Oficio oficio;
	public bool hornoEncendido;
	private bool mirandoPlayer; // Variable para controlar si el NPC está mirando al jugador
	public Transform rutaTrazada;

	public Inventario npcInventario;
	public MesasArtesanos almacenInv;
	private MoverAgente agente;
	private bool moviendo; // Variable para controlar si el NPC está en movimiento

	[Header("Rutas de los artesanos")]
	public Transform[] rutasMediodia;

	private void Start()
	{
		agente = GetComponent<MoverAgente>(); // Obtener el componente MoverAgente para mover al NPC
		npcInventario = GetComponent<Inventario>(); // Obtener el inventario del NPC

		hornoEncendido = false; // Por defecto el horno está apagado

		switch (oficio)
		{
			case Oficio.Herrero:
				CambiarEstado(new EstadoHerrero(this));
				break;
			case Oficio.Panadero:
				CambiarEstado(new EstadoPanadero(this));
				break;
				// Otros casos...
		}
	}

	
	public void CambiarEstado(IOficioState nuevoEstado)
	{
		if (estadoActual != null)
		{
			StopAllCoroutines();
		}

		estadoActual = nuevoEstado;
		StartCoroutine(estadoActual.EjecutarRutina());
	}

	private void Update()
	{
		// Solo para ver a dónde se mueve el NPC
		if (rutaTrazada != null)
		{
			Debug.DrawLine(transform.position, rutaTrazada.position, Color.red);
		}

		// Verificamos si el NPC no está en movimiento y si hay rutas disponibles.
		if (!moviendo && rutaTrazada != null)
		{
			// Mover al NPC al punto actual de la ruta.
			agente.MoverA(rutaTrazada);
			moviendo = true;
		}

		// Si el NPC ha llegado a la ruta, hacemos una acción.
		if (moviendo && agente.HaLlegado())
		{
			moviendo = false;  // Reseteamos el estado de movimiento.

			// Aquí podrías agregar alguna acción adicional cuando el NPC llegue a la ruta, 
			// por ejemplo, permitir al jugador seleccionar el siguiente destino.
		}
	}

	// Método para que el NPC mire al jugador cuando está cerca
	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			mirandoPlayer = true;
			RotateTowardsTarget(other.transform.position);
		}
	}
	private void OnTriggerExit(Collider other)
	{
		mirandoPlayer = false;
	}

	// Método para rotar al NPC hacia una posición objetivo suavemente
	public void RotateTowardsTarget(Vector3 targetPosition)
	{
		// Calcular la dirección hacia el objetivo
		Vector3 directionToTarget = targetPosition - transform.position;
		directionToTarget.y = 0; // Ignorar la diferencia en el eje Y

		// Crear una rotación que mire hacia el objetivo
		if (directionToTarget.sqrMagnitude > 0.0001f) // Usar sqrMagnitude para evitar cálculos innecesarios
		{
			Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

			// Aplicar la rotación suavemente sin afectar el NavMeshAgent
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);
		}
	}

	// Método auxiliar para esperar con pausa usando la variable "mirandoPlayer"
	public IEnumerator EsperarConPausa(float tiempo)
	{
		float tiempoTranscurrido = 0f;

		while (tiempoTranscurrido < tiempo)
		{
			if (mirandoPlayer)
			{
				// Pausar hasta que "mirandoPlayer" sea falso
				yield return new WaitUntil(() => !mirandoPlayer);
			}

			yield return null; // Esperar un frame
			tiempoTranscurrido += Time.deltaTime;
		}
	}
}
