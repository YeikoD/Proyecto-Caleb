using System.Collections;
using UnityEngine;

public class EstadoHerrero : IOficioState
{
	private OficioSystems npc;

	// Cacheo de referencias a los ítems
	public ItemData hierro, lingoteHierro, madera;

	// Reemplaza los literales por constantes para los recursos
	private const string RECURSO_MADERA = "madera";
	private const string RECURSO_HIERRO = "hierro";
	private const string RECURSO_LINGOTE_HIERRO = "lingoteHierro";

	public EstadoHerrero(OficioSystems npc)
	{
		this.npc = npc;
		hierro = ItemDB.Instancia.ObtenerItemPorNombre("Hierro");
		lingoteHierro = ItemDB.Instancia.ObtenerItemPorNombre("LingoteHierro");
		madera = ItemDB.Instancia.ObtenerItemPorNombre("Madera");
	}

	public IEnumerator EjecutarRutina()
	{
		yield return npc.EsperarConPausa(5f);
		Debug.Log("[EstadoHerrero] Re / Iniciando rutina...");

		if (npc.hornoEncendido)
		{
			yield return ProcesarHornoEncendido();
		}
		else
		{
			yield return IntentarEncenderHorno();
		}

		// Repite la rutina de herrero
		yield return npc.EsperarConPausa(5f);
		npc.CambiarEstado(new EstadoHerrero(npc));
	}

	private IEnumerator ProcesarHornoEncendido()
	{
		Debug.Log("[EstadoHerrero] Comprobando los recursos del almacén");
		npc.rutaTrazada = npc.rutasMediodia[1]; // Almacén
		yield return npc.EsperarConPausa(5f);

		if (npc.npcInventario.ObtenerCantidad(hierro) > 0)
		{
			yield return FundirHierro();
		}
		else
		{
			yield return ObtenerRecurso(hierro, RECURSO_HIERRO, 5, 10, 25);
		}
	}

	private IEnumerator FundirHierro()
	{
		Debug.Log("[EstadoHerrero] Se dirige a fundir el hierro");
		npc.rutaTrazada = npc.rutasMediodia[2]; // Horno
		yield return npc.EsperarConPausa(Random.Range(50, 70));

		npc.npcInventario.QuitarItem(hierro, 1);
		npc.npcInventario.AgregarItem(lingoteHierro, 1);

		npc.rutaTrazada = npc.rutasMediodia[1]; // Almacén
		yield return npc.EsperarConPausa(5f);

		if (npc.npcInventario.ObtenerCantidad(lingoteHierro) > 0 && npc.mesa.almacenInv.ObtenerCantidad(lingoteHierro) < 6)
		{
			npc.npcInventario.QuitarItem(lingoteHierro, 1);
			npc.mesa.almacenInv.AgregarItem(lingoteHierro, 1);
			Debug.Log("[EstadoHerrero] Lingote de hierro almacenado.");
		}
	}

	private IEnumerator IntentarEncenderHorno()
	{
		Debug.Log("[EstadoHerrero] El horno está apagado. Intentando encenderlo");

		if (npc.npcInventario.ObtenerCantidad(madera) > 0)
		{
			yield return EncenderHorno();
		}
		else
		{
			yield return ObtenerRecurso(madera, RECURSO_MADERA, 5, 10, 25);
			if (npc.npcInventario.ObtenerCantidad(madera) > 0)
			{
				yield return EncenderHorno();
			}
			else
			{
				Debug.Log("[EstadoHerrero] No hay madera para encender el horno. Esperando...");
			}
		}
	}

	private IEnumerator EncenderHorno()
	{
		npc.rutaTrazada = npc.rutasMediodia[2]; // Horno
		yield return npc.EsperarConPausa(Random.Range(4, 7));

		npc.npcInventario.QuitarItem(madera, 1);
		npc.hornoEncendido = true; // Cambia el estado del horno a encendido
		Debug.Log("[EstadoHerrero] Horno encendido. Comenzando a trabajar...");
	}

	private IEnumerator ObtenerRecurso(ItemData item, string recurso, float pausaAntesSolicitar, float pausaDespuesSolicitar, float pausaFinal)
	{
		if (npc.mesa.almacenInv.ObtenerCantidad(item) > 0)
		{
			npc.mesa.almacenInv.QuitarItem(item, 1);
			npc.npcInventario.AgregarItem(item, 1);
			Debug.Log($"[EstadoHerrero] {recurso} obtenido del almacén.");
		}
		else
		{
			Debug.Log($"[EstadoHerrero] No hay {recurso}. Solicitando...");
			npc.rutaTrazada = npc.rutasMediodia[5]; // Pedir recurso al soberano
			yield return npc.EsperarConPausa(pausaAntesSolicitar);

			EventManager.TriggerEvent("InicioTransporte", ("herrero", recurso));
			npc.rutaTrazada = npc.rutasMediodia[0]; // Esperar en el taller
			yield return npc.EsperarConPausa(pausaDespuesSolicitar);
			yield return npc.EsperarConPausa(pausaFinal);
		}
	}

	public void RecibirRecurso((string recurso, int cantidad) eventData)
	{
		Debug.Log($"[EstadoHerrero] Evento 'EntregarRecursoHerrero' recibido con recurso: {eventData.recurso}, cantidad: {eventData.cantidad}");

		if (eventData.recurso == RECURSO_MADERA)
		{
			npc.mesa.almacenInv.AgregarItem(madera, eventData.cantidad);
			Debug.Log($"[EstadoHerrero] Madera recibida por repartidor: {eventData.cantidad} unidades.");
		}
		else if (eventData.recurso == RECURSO_HIERRO)
		{
			npc.mesa.almacenInv.AgregarItem(hierro, eventData.cantidad);
			Debug.Log($"[EstadoHerrero] Hierro recibido por repartidor: {eventData.cantidad} unidades.");
		}
		else
		{
			Debug.LogWarning($"[EstadoHerrero] Recurso '{eventData.recurso}' no reconocido.");
		}
	}
}