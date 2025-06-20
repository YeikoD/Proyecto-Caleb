using UnityEngine;

public enum SiervoOficio
{
	Transportista,
	Domestico
}

public class SiervosSystems : NPCBaseSystems
{
	[Header("Configuracion")]
	private ISiervoState estadoActual;
	public SiervoOficio siervoOficio;
	public string solicitudType;
	public bool irARecoger = false;

	[Header("Referencias")]
	public MesaInteraccion mesa; // Inventario de alamcenamiento
	public Transform[] rutasTrabajo;

	protected override void Start()
	{
		base.Start();

		switch (siervoOficio)
		{
			case SiervoOficio.Transportista:
				estadoActual = new EstadoSiervoTransportista(this); // Inicializa el estado
				break;
			case SiervoOficio.Domestico:
				// estadoActual = new EstadoSiervoDomestico(this); // Si tienes otro estado, inicialízalo aquí
				break;
		}
	}

	private void OnEnable()
	{
		// Aquí se crea el evento "OnPlayerDeath" si no existe
		EventManager.AddListener<(string, string)>("InicioTransporte", TransportarRecurso);
	}

	private void OnDisable()
	{
		// Eliminamos el listener cuando el objeto se desactiva
		EventManager.RemoveListener<(string, string)>("InicioTransporte", TransportarRecurso);
	}

	// Método que maneja el evento
	private void TransportarRecurso((string solicitante, string recurso) eventData)
	{
		StartCoroutine(estadoActual.EjecutarRutina(eventData.solicitante, eventData.recurso));
	}
}
