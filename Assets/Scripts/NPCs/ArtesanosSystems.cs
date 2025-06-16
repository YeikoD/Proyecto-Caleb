using System.Collections;
using UnityEngine;

// Tipos de NPC
public enum NpcOficio
{
	Herrero,
	Panadero,
	Carpintero,
	Yunque
}

public class ArtesanosSystems : MonoBehaviour
{
	[SerializeField] private MoverAgente npc;                                       // Referencia al script MoverAgente que controla el movimiento del NPC.
	[SerializeField] private ItemDB itemDb;                                         // Base de datos de items para obtener referencias a los items por nombre.
	[SerializeField] private Transform rutaTrazada;                                 // Arreglo de rutas por las que el NPC se moverá.
	[SerializeField] private Inventario npcInventario;                              // Referencia al inventario del NPC.
	[SerializeField] private NpcOficio npcOficio;                                   // Tipo de trabajo actual del NPC.

	public bool hornoEncendido;

	private bool moviendo = false;                                                  // Indica si el NPC está en movimiento.
	public bool mirandoPlayer = false;

	public Transform[] rutasAmanecer, rutasMediodia, rutasNoche;                    // Todas las rutas que tomara el NPC durante el dia
	private ItemData hierro, madera, hierroForjado, harina, masa, pan;      // Cachear referencias a los items para evitar búsquedas repetidas.

	private void Start()
	{
		npcInventario = GetComponent<Inventario>(); // Obtenemos el componente Inventario del NPC.

		// Cachear referencias a los items para evitar búsquedas repetidas
		hierro = itemDb.ObtenerItemPorNombre("Hierro");
		madera = itemDb.ObtenerItemPorNombre("Madera");
		hierroForjado = itemDb.ObtenerItemPorNombre("HierroForjado");
		harina = itemDb.ObtenerItemPorNombre("Harina");
		masa = itemDb.ObtenerItemPorNombre("Masa");
		pan = itemDb.ObtenerItemPorNombre("Pan");

		StartCoroutine(RutinaMediodia()); // Iniciamos la rutina del NPC al mediodía.
	}

	void Update()
	{
		// Verificamos si el NPC no está en movimiento y si hay rutas disponibles.
		if (!moviendo && rutaTrazada != null)
		{
			// Mover al NPC al punto actual de la ruta.
			npc.MoverA(rutaTrazada);
			moviendo = true;
		}

		// Si el NPC ha llegado a la ruta, hacemos una acción.
		if (moviendo && npc.HaLlegado())
		{
			moviendo = false;  // Reseteamos el estado de movimiento.

			// Aquí podrías agregar alguna acción adicional cuando el NPC llegue a la ruta, 
			// por ejemplo, permitir al jugador seleccionar el siguiente destino.
		}
	}

	IEnumerator RutinaMediodia()
	{
		switch (npcOficio)
		{
			case NpcOficio.Herrero:
				yield return RutinaHerrero();
				break;
			case NpcOficio.Panadero:
				yield return RutinaPanadero();
				break;
			case NpcOficio.Carpintero:
				yield return RutinaCarpintero();
				break;
			// Puedes agregar más casos según el enum
			default:
				yield return EsperarConPausa(5f);
				break;
		}
	}

	// Rutina específica para el Herrero
	IEnumerator RutinaHerrero()
	{
		Debug.Log("[NPC] Re/Inicio de la rutina mediodía (Herrero).");
		yield return new WaitForSeconds(2f); // Espera inicial para simular el inicio de la rutina

		if (hornoEncendido)
		{
			// NO tiene hierro...
			if (npcInventario.ObtenerCantidad(hierro) < 1)
			{
				Debug.Log("[NPC] Herrero: Buscando hierro.");
				rutaTrazada = rutasMediodia[1]; // Se dirige a buscar hierro al almacen
				yield return EsperarConPausa(5f);
				Debug.Log("[NPC] Herrero: Hierro obtenido.");
			}
			else
			{
				Debug.Log("[NPC] Herrero: Fundiendo hierro.");
				rutaTrazada = rutasMediodia[3]; // Se dirige al horno a fundir hierro
				yield return EsperarConPausa(10f);

				rutaTrazada = rutasMediodia[0]; // Espera en el taller
				Debug.Log("[NPC] Herrero: Esperando.");
				yield return EsperarConPausa(30);

				rutaTrazada = rutasMediodia[3]; // Se dirige al horno a tomar el hierro forjado
				Debug.Log("[NPC] Herrero: Hierro forjado.");
				yield return EsperarConPausa(5f);

				rutaTrazada = rutasMediodia[1]; // Se dirige al almacen a guardar el hierro forjado
				Debug.Log("[NPC] Herrero: Guardando hierro forjado.");
				yield return EsperarConPausa(3f);
			}
		}
		else
		{
			// NO tiene madera...
			if (npcInventario.ObtenerCantidad(madera) < 1 && !hornoEncendido)
			{
				Debug.Log("[NPC] Herrero: Buscando carbón.");
				rutaTrazada = rutasMediodia[1]; // Se dirige a buscar carbón al almacen
				yield return EsperarConPausa(3f);
				Debug.Log("[NPC] Herrero: Carbón obtenido.");
			}
			else
			{
				Debug.Log("[NPC] Herrero: Yendo a encender el horno.");
				rutaTrazada = rutasMediodia[3]; // Se dirige al horno
				yield return EsperarConPausa(5f);
				Debug.Log("[NPC] Herrero: Horno encendido.");
			}
		}

		yield return EsperarConPausa(5f); // Espera antes de repetir la rutina
		StartCoroutine(RutinaHerrero());
	}

	// Ejemplo de rutina para Panadero (puedes personalizarla):
	IEnumerator RutinaPanadero()
	{
		Debug.Log("[NPC] Re/Inicio de la rutina mediodía (Panadero).");
		yield return new WaitForSeconds(2f); // Espera inicial para simular el inicio de la rutina

		if (hornoEncendido)
		{

			if (npcInventario.ObtenerCantidad(harina) > 0)
			{
				Debug.Log("[NPC] Panadero: Haciendo masa.");
				rutaTrazada = rutasMediodia[2]; // Se dirige a la mesa de trabajo para hacer masa
				yield return EsperarConPausa(15f);

				if (npcInventario.ObtenerCantidad(masa) > 0)
				{
					Debug.Log("[NPC] Panadero: Horneando pan.");
					rutaTrazada = rutasMediodia[3];
					yield return EsperarConPausa(5);

					rutaTrazada = rutasMediodia[0]; // Se dirige esperar
					Debug.Log("[NPC] Panadero: Esperando.");
					yield return EsperarConPausa(20f);

					rutaTrazada = rutasMediodia[3]; // Se dirige al horno a recoger el pan
					Debug.Log("[NPC] Panadero: Pan horneado.");
					yield return EsperarConPausa(5f);

					rutaTrazada = rutasMediodia[1]; // Se dirige al almacen a guardar el pan
					yield return EsperarConPausa(3f);
				}
			}
			else
			{
				Debug.Log("[NPC] Panadero: Buscando harina.");
				rutaTrazada = rutasMediodia[1]; // Almacen para buscar harina
				yield return EsperarConPausa(5f);
				Debug.Log("[NPC] Panadero: Harina obtenida.");
			}
		}
		else
		{
			Debug.Log("[NPC] Panadero: Yendo al taller para encender el horno.");
			rutaTrazada = rutasMediodia[1]; // Se dirige al almacen para buscar madera
			yield return EsperarConPausa(4f);

			if (npcInventario.ObtenerCantidad(madera) > 0)
			{
				Debug.Log("[NPC] Panadero: Encendiendo el horno.");
				rutaTrazada = rutasMediodia[3]; // Se dirige a encender el horno	
				yield return EsperarConPausa(5f);
			}
			else
			{
				Debug.Log("[NPC] Panadero: No tiene madera para encender el horno.");
				rutaTrazada = rutasMediodia[0]; // Se quedo sin rutina, se dirige esperar
				yield return EsperarConPausa(20f);
			}
		}

		yield return EsperarConPausa(5f);
		StartCoroutine(RutinaPanadero()); // Repite la rutina
	}

	// Rutina específica para el Carpintero (ejemplo)
	IEnumerator RutinaCarpintero()
	{
		Debug.Log("[NPC] Inicio de la rutina mediodía (Carpintero).");
		// Aquí iría la lógica específica del carpintero
		yield return EsperarConPausa(8f);
	}

	// Método auxiliar para esperar con pausa usando la variable "mirandoPlayer"
	IEnumerator EsperarConPausa(float tiempo)
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
}
