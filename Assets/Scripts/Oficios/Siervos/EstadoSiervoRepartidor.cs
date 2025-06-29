using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Administra el sistema de reparto de la aldea.
/// </summary>
public class EstadoSiervoRepartidor : MonoBehaviour
{
	public static EstadoSiervoRepartidor Instancia { get; private set; }

	private Queue<SolicitudDeReparto> colaSolicitudes = new();

	public bool RepartidorOcupado { get; private set; } = false;

	public SolicitudDeReparto SolicitudPendienteActiva { get; private set; } = null;

	private void Awake()
	{
		if (Instancia != null && Instancia != this)
		{
			Destroy(gameObject);
			return;
		}
		Instancia = this;
	}

	/// <summary>
	/// Solicita un item al repartidor. Se agrega a la cola.
	/// </summary>
	public bool SolicitarItem(string idItem, int cantidad, NPC solicitante)
	{
		if (solicitante == null || string.IsNullOrEmpty(idItem) || cantidad <= 0)
			return false;

		var solicitud = new SolicitudDeReparto
		{
			ItemID = idItem,
			Cantidad = cantidad,
			QuienPidio = solicitante
		};

		colaSolicitudes.Enqueue(solicitud);
		Debug.Log($"[Repartidor] Nueva solicitud: {cantidad}x '{idItem}' para {solicitante.Nombre}");
		return true;
	}

	/// <summary>
	/// Indica si hay solicitudes en espera.
	/// </summary>
	public bool HaySolicitudesPendientes() => colaSolicitudes.Count > 0;

	/// <summary>
	/// Indica si hay una entrega en proceso.
	/// </summary>
	public bool HayTareaActiva() => RepartidorOcupado;

	/// <summary>
	/// El repartidor toma la siguiente solicitud para procesarla.
	/// </summary>
	public SolicitudDeReparto ObtenerSiguienteSolicitud()
	{
		if (!HaySolicitudesPendientes())
		{
			SolicitudPendienteActiva = null;
			RepartidorOcupado = false;
			return null;
		}

		SolicitudPendienteActiva = colaSolicitudes.Dequeue();
		RepartidorOcupado = true;
		Debug.Log($"[Repartidor] Atendiendo solicitud: {SolicitudPendienteActiva.Cantidad}x '{SolicitudPendienteActiva.ItemID}' para {SolicitudPendienteActiva.QuienPidio.Nombre}");
		return SolicitudPendienteActiva;
	}

	/// <summary>
	/// Marca al repartidor como libre después de entregar.
	/// </summary>
	public void MarcarComoDisponible()
	{
		SolicitudPendienteActiva = null;
		RepartidorOcupado = false;
		Debug.Log("[Repartidor] Ha quedado disponible para nuevas solicitudes.");
	}
}

[System.Serializable]
public class SolicitudDeReparto
{
	public string ItemID;
	public int Cantidad;
	public NPC QuienPidio;
}
