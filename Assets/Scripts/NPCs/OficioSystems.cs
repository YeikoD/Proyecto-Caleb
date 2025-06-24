using System.Collections;
using UnityEngine;

public enum NpcType
{
    Campesino, // NPC campesino
    Siervo // NPC siervo
}

public enum CampesinoOficio
{
    Ninguno, Herrero, Panadero
} 

public enum SiervoOifico
{
    Ninguno, Repartidor, Gestionador
}

public class OficioSystems : NPCBaseSystems
{
    private IOficioState estadoActual;

	[Header("Info")]
	public bool hornoEncendido;				// Indica si el horno está encendido o no
    public NpcType npcType;					// Tipo de NPC (Campesino o
	public CampesinoOficio campesinoOficio; // Tipo de oficio del campesino
    public SiervoOifico siervoOficio;       // Tipo de oficio del siervo
	public bool mirandoPlayer;

	[Header("Referencias")]
	public PuestoTrabajo puestoTrabajo;			// Inventario de alamcenamiento

	[Header("Rutas de los artesanos")]
    public Transform[] rutasMediodia;

	public EstadoSiervoRepartidor estadoSiervoRepartidor; 
	private EstadoHerrero estadoHerrero;
	private	EstadoPanadero estadoPanadero;

	protected override void Start()
    {
        base.Start();
        hornoEncendido = false; // Por defecto el horno está apagado

		// Inicializar el estado del NPC según su tipo y oficio
		switch (npcType)
        {
			// --- CAMPESINO ---------------------------------------------------------------------
			case NpcType.Campesino:
                switch (campesinoOficio)
                {
                    case CampesinoOficio.Herrero:
                        estadoHerrero = new EstadoHerrero(this);
                        CambiarEstado(estadoHerrero);
						puestoTrabajo = ReferenciasGlobales.Instancia.PUESTO_HERRERO; // Asignar el puesto de trabajo del herrero
						break;
                    case CampesinoOficio.Panadero:
                        estadoPanadero = new EstadoPanadero(this);
                        CambiarEstado(estadoPanadero);
						puestoTrabajo = ReferenciasGlobales.Instancia.PUESTO_PANADERO; // Asignar el puesto de trabajo del panadero
						break;
                    case CampesinoOficio.Ninguno:
                        // No hacer nada
                        break;
                    default:
                        Debug.LogWarning("Oficio de campesino no reconocido.");
                        break;
				}
				break;
			// --- SIERVO ---------------------------------------------------------------------
			case NpcType.Siervo:
				switch (siervoOficio)
				{
					case SiervoOifico.Ninguno:
						break;
					case SiervoOifico.Repartidor:
						estadoSiervoRepartidor = new EstadoSiervoRepartidor(this);
						CambiarEstado(estadoSiervoRepartidor);
						puestoTrabajo = ReferenciasGlobales.Instancia.PUESTO; // Asignar el puesto de trabajo del repartidor
						break;
					case SiervoOifico.Gestionador:
						break;
					default:
						break;
				}
				break;
            default:
                break;
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

	private void OnEnable()
	{
		// Inicializar instancias si no están ya inicializadas
        if (campesinoOficio == CampesinoOficio.Herrero && estadoHerrero == null)
            estadoHerrero = new EstadoHerrero(this);
        if (campesinoOficio == CampesinoOficio.Panadero && estadoPanadero == null)
            estadoPanadero = new EstadoPanadero(this);
        if (npcType == NpcType.Siervo && siervoOficio == SiervoOifico.Repartidor && estadoSiervoRepartidor == null)
            estadoSiervoRepartidor = new EstadoSiervoRepartidor(this);

		EventManager.AddListener<(string, string)>("InicioTransporte", TransportarRecurso);

		if (estadoHerrero != null)
			EventManager.AddListener<(string, int)>("EntregarRecursoHerrero", estadoHerrero.RecibirRecurso);
		if (estadoPanadero != null)
			EventManager.AddListener<(string, int)>("EntregarRecursoPanadero", estadoPanadero.RecibirRecurso);
		if (estadoSiervoRepartidor != null)
			EventManager.AddListener<(string, int)>("EntregarRecursoRepartidor", estadoSiervoRepartidor.RecibirRecurso);

		Debug.Log("[OficioSystems] Listeners de entrega de recursos registrados.");
	}

	private void OnDisable()
	{
		// Eliminamos el listener cuando el objeto se desactiva
		EventManager.RemoveListener<(string, string)>("InicioTransporte", TransportarRecurso);

		if (estadoHerrero != null)
			EventManager.RemoveListener<(string, int)>("EntregarRecursoHerrero", estadoHerrero.RecibirRecurso); 
		if (estadoPanadero != null)
			EventManager.RemoveListener<(string, int)>("EntregarRecursoPanadero", estadoPanadero.RecibirRecurso);
		if (estadoSiervoRepartidor != null)
			EventManager.RemoveListener<(string, int)>("EntregarRecursoRepartidor", estadoSiervoRepartidor.RecibirRecurso);
	}

	// Evento solicitar recurso a al repartidor. NPC_Siervo
	private void TransportarRecurso((string solicitante, string recurso) eventData)
	{
		if (estadoActual is EstadoSiervoRepartidor repartidor)
		{
			repartidor.ConfigurarParametros(eventData.solicitante, eventData.recurso);
			StopAllCoroutines(); // Detén cualquier corutina previa
			StartCoroutine(estadoActual.EjecutarRutina());
		}
	}

	public IEnumerator EsperarConPausa(float tiempo)
	{
		float tiempoRestante = tiempo;

		while (tiempoRestante > 0)
		{
			if (mirandoPlayer)
			{
				Debug.Log("[EstadoPanadero] Pausa activada. Esperando...");
				PausarMovimiento(true); // Detener el movimiento del NPC
				while (mirandoPlayer)
				{
					yield return null; // Espera mientras mira al jugador
				}
				PausarMovimiento(false); // Reanudar el movimiento del NPC
				Debug.Log("[EstadoPanadero] Reanudando rutina.");
			}

			yield return null;
			tiempoRestante -= Time.deltaTime;
		}

		Debug.Log("[EstadoPanadero] Espera completada.");
	}

	public void PausarMovimiento(bool pausar)
	{
		if (agente == null)
		{
			Debug.LogError("[OficioSystems] NavMeshAgent no asignado.");
			return;
		}

		agente.DetenerMovimiento(pausar);

		if (pausar)
		{
			Debug.Log("[OficioSystems] Movimiento del NPC pausado.");
		}
		else
		{
			Debug.Log("[OficioSystems] Movimiento del NPC reanudado.");
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			mirandoPlayer = true;
			RotateTowardsTarget(other.transform.position);

			// Pausar el movimiento del NPC
			PausarMovimiento(true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			mirandoPlayer = false;

			// Reanudar el movimiento del NPC
			PausarMovimiento(false);
		}
	}
}
