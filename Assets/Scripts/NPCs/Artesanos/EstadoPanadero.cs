using System.Collections;
using UnityEngine;

public class EstadoPanadero : IOficioState
{
	private OficioSystems npc;

	// Cacheo de referencias a los ítems
	private ItemData madera, harina, masa, pan;

	// Reemplaza los literales por constantes para los recursos
	private const string RECURSO_MADERA = "madera";
	private const string RECURSO_HARINA = "harina";

	public EstadoPanadero(OficioSystems npc)
	{
		this.npc = npc;
		// Cachear referencias a los ítems
		pan = ItemDB.Instancia.ObtenerItemPorNombre("Pan");
		harina = ItemDB.Instancia.ObtenerItemPorNombre("Harina");
		masa = ItemDB.Instancia.ObtenerItemPorNombre("Masa");
		madera = ItemDB.Instancia.ObtenerItemPorNombre("Madera");
	}

	public IEnumerator EjecutarRutina()
	{
		yield return npc.EsperarConPausa(5f);
		Debug.Log("[EstadoPanadero] Re / Iniciando rutina...");

		if (npc.hornoEncendido)
		{
			Debug.Log("[EstadoPanadero] Comprobando los recursos del almacén");
			npc.rutaTrazada = npc.rutasMediodia[1]; // Almacén
			yield return npc.EsperarConPausa(5f);

			if (npc.npcInventario.ObtenerCantidad(harina) > 0)
			{
				yield return PrepararPan();
			}
			else
			{
				yield return ObtenerRecursoDeAlmacen(harina, RECURSO_HARINA, 10f, 4, 25f, 0, "[EstadoPanadero] se ha tomado harina del almacén.", "[EstadoPanadero] No hay harina. Solicitando...", "InicioTransporte", "[EstadoPanadero] No hay harina en el almacén.");
			}
		}
		else
		{
			Debug.Log("[EstadoPanadero] El horno está apagado. Intentando encenderlo");

			if (npc.npcInventario.ObtenerCantidad(madera) > 0)
			{
				yield return EncenderHorno();
			}
			else
			{
				yield return ObtenerRecursoDeAlmacen(madera, RECURSO_MADERA, 10f, 4, 25f, 3, "[EstadoPanadero] Madera obtenida del almacén.", "[EstadoPanadero] No hay madera. Solicitando...", "InicioTransporte", "[EstadoPanadero] No hay madera en el almacén.");
			}
		}

		yield return npc.EsperarConPausa(5f);
		npc.CambiarEstado(new EstadoPanadero(npc));
	}

	private IEnumerator PrepararPan()
	{
		Debug.Log("[EstadoPanadero] Se dirige a preprar masa");
		npc.rutaTrazada = npc.rutasMediodia[2]; // Mesa de amasar
		yield return npc.EsperarConPausa(30f);

		npc.npcInventario.QuitarItem(harina, 1);
		npc.npcInventario.AgregarItem(masa, 1);

		Debug.Log("[EstadoPanadero] Yendo a hornear la masa");
		npc.rutaTrazada = npc.rutasMediodia[3]; // Horno
		yield return npc.EsperarConPausa(10f);

		Debug.Log("[EstadoPanadero] pan horneandose...");
		npc.rutaTrazada = npc.rutasMediodia[0]; // Panadería

		yield return npc.EsperarConPausa(20f);

		npc.npcInventario.QuitarItem(masa, 1);
		npc.npcInventario.AgregarItem(pan, 1);

		Debug.Log("[EstadoPanadero] Pan horneado. se dirige a guaradar en alamacen");
		npc.rutaTrazada = npc.rutasMediodia[1]; // Almacén
		yield return npc.EsperarConPausa(5f);

		if (npc.npcInventario.ObtenerCantidad(pan) > 0)
		{
			npc.npcInventario.QuitarItem(pan, 1);
			npc.mesa.almacenInv.AgregarItem(pan, 1);
			Debug.Log("[EstadoPanadero] Masa almacenada.");
		}
	}

	private IEnumerator EncenderHorno()
	{
		npc.rutaTrazada = npc.rutasMediodia[3]; // Horno
		yield return npc.EsperarConPausa(5f);

		npc.npcInventario.QuitarItem(madera, 1);
		npc.hornoEncendido = true; // Cambia el estado del horno a encendido
		Debug.Log("[EstadoPanadero] Horno encendido. Comenzando a trabajar...");
	}

	// Método auxiliar para obtener recursos del almacén o solicitarlos
	private IEnumerator ObtenerRecursoDeAlmacen(ItemData item, string recurso, float pausaAntesSolicitar, int rutaSolicitar, float pausaDespuesSolicitar, int rutaFinal, string logObtenido, string logSolicitando, string eventoSolicitar, string logNoHay)
	{
		if (npc.mesa.almacenInv.ObtenerCantidad(item) > 0)
		{
			npc.mesa.almacenInv.QuitarItem(item, 1);
			npc.npcInventario.AgregarItem(item, 1);
			Debug.Log(logObtenido);

			// Si el recurso es harina, continuar el flujo de preparación de pan
			if (item == harina)
			{
				yield return PrepararPan();
			}
		}
		else
		{
			Debug.Log(logNoHay);
			npc.rutaTrazada = npc.rutasMediodia[rutaSolicitar]; // Pedir recurso al soberano
			yield return npc.EsperarConPausa(pausaAntesSolicitar);

			EventManager.TriggerEvent(eventoSolicitar, ("panadero", recurso));
			npc.rutaTrazada = npc.rutasMediodia[rutaFinal]; // Esperar en la panadería

			yield return npc.EsperarConPausa(pausaDespuesSolicitar);
		}
	}

	public void RecibirRecurso((string recurso, int cantidad) eventData)
	{
		Debug.Log($"[EstadoPanadero] Evento 'EntregarRecursoPanadero' recibido con recurso: {eventData.recurso}, cantidad: {eventData.cantidad}");

		if (eventData.recurso == RECURSO_MADERA)
		{
			npc.mesa.almacenInv.AgregarItem(madera, eventData.cantidad);
			Debug.Log($"[EstadoPanadero] Madera recibida por repartidor: {eventData.cantidad} unidades.");
		}
		else if (eventData.recurso == RECURSO_HARINA)
		{
			npc.mesa.almacenInv.AgregarItem(harina, eventData.cantidad);
			Debug.Log($"[EstadoPanadero] Harina recibida por repartidor: {eventData.cantidad} unidades.");
		}
		else
		{
			Debug.LogWarning($"[EstadoPanadero] Recurso '{eventData.recurso}' no reconocido.");
		}
	}
}