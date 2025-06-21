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

	// Constructor que recibe el NPC y cachea las referencias a los ítems necesarios
	public EstadoPanadero(OficioSystems npc)
	{
		this.npc = npc;
		// Cachear referencias a los ítems
		pan = ItemDB.Instancia.ObtenerItemPorNombre("Pan");
		harina = ItemDB.Instancia.ObtenerItemPorNombre("Harina");
		masa = ItemDB.Instancia.ObtenerItemPorNombre("Masa");
		madera = ItemDB.Instancia.ObtenerItemPorNombre("Madera");

	}

	// Método que ejecuta la rutina del panadero
	public IEnumerator EjecutarRutina()
	{
		yield return npc.EsperarConPausa(5f);
		Debug.Log("[EstadoPanadero] Re / Iniciando rutina...");

		if (npc.hornoEncendido)
		{
			Debug.Log("[EstadoPanadero] Comprobando los recursos del almacén");
			npc.rutaTrazada = npc.rutasMediodia[1]; // Almacén
			yield return npc.EsperarConPausa(5f);

			// Si hay harina en el inventario del NPC
			if (npc.npcInventario.ObtenerCantidad(harina) > 0)
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

				// Si hay pan en el inventario del NPC almacena
				if (npc.npcInventario.ObtenerCantidad(pan) > 0)
				{
					npc.npcInventario.QuitarItem(pan, 1);
					npc.mesa.almacenInv.AgregarItem(pan, 1);
					Debug.Log("[EstadoPanadero] Masa almacenada.");
				}
			}
			else
			{
				if (npc.mesa.almacenInv.ObtenerCantidad(harina) > 0)
				{
					npc.mesa.almacenInv.QuitarItem(harina, 1);
					npc.npcInventario.AgregarItem(harina, 1);
					Debug.Log("[EstadoPanadero] se ha tomado harina del almacén.");
				}
				else
				{
					Debug.Log("[EstadoPanadero] No hay harina. Solicitando...");
					npc.rutaTrazada = npc.rutasMediodia[4]; // Pedir harina al soberano

					yield return npc.EsperarConPausa(10f);

					EventManager.TriggerEvent("InicioTransporte", ("panadero", RECURSO_HARINA));
					npc.rutaTrazada = npc.rutasMediodia[0]; // Esperar en la panadería

					yield return npc.EsperarConPausa(25f);
				}
			}
		}
		else
		{
			Debug.Log("[EstadoPanadero] El horno está apagado. Intentando encenderlo");

			// Intenta encender el horno
			if (npc.npcInventario.ObtenerCantidad(madera) > 0)
			{
				npc.rutaTrazada = npc.rutasMediodia[3]; // Horno

				yield return npc.EsperarConPausa(5f);

				npc.npcInventario.QuitarItem(madera, 1);
				npc.hornoEncendido = true; // Cambia el estado del horno a encendido
				Debug.Log("[EstadoPanadero] Horno encendido. Comenzando a trabajar...");
			}
			else
			{
				Debug.Log("[EstadoPanadero] No hay madera. Buscando en el almacén...");
				npc.rutaTrazada = npc.rutasMediodia[1]; // Almacén
				yield return npc.EsperarConPausa(5f);

				// Verifica si hay madera en el almacén
				if (npc.mesa.almacenInv.ObtenerCantidad(madera) > 0)
				{
					npc.mesa.almacenInv.QuitarItem(madera, 1);
					npc.npcInventario.AgregarItem(madera, 1);

					Debug.Log("[EstadoPanadero] Madera obtenida del almacén.");
				}
				else
				{
					Debug.Log("[EstadoPanadero] No hay madera en el almacén.");
				}

				npc.rutaTrazada = npc.rutasMediodia[3]; // Horno
				yield return npc.EsperarConPausa(5f);

				// Intenta encender el horno con la madera obtenida
				if (npc.npcInventario.ObtenerCantidad(madera) > 0)
				{
					npc.npcInventario.QuitarItem(madera, 1);
					npc.hornoEncendido = true; // Cambia el estado del horno a encendido
					Debug.Log("[EstadoPanadero] Horno encendido. Comenzando a trabajar...");
				}
				else
				{
					Debug.Log("[EstadoPanadero] No hay madera. Solicitando...");
					npc.rutaTrazada = npc.rutasMediodia[4]; // Pedir madera al soberano

					yield return npc.EsperarConPausa(10f);

					EventManager.TriggerEvent("InicioTransporte", ("panadero", RECURSO_MADERA));
					npc.rutaTrazada = npc.rutasMediodia[0]; // Esperar en la panadería

					yield return npc.EsperarConPausa(25f);
				}
			}
		}

		// Repite la rutina de panadero
		yield return npc.EsperarConPausa(5f);
		npc.CambiarEstado(new EstadoPanadero(npc));
	}

	public void RecibirRecurso((string recurso, int cantidad) eventData)
	{
		Debug.Log($"[EstadoPanadero] Evento 'EntregarRecursoPanadero' recibido con recurso: {eventData.recurso}, cantidad: {eventData.cantidad}");

		if (eventData.recurso == "madera")
		{
			npc.mesa.almacenInv.AgregarItem(madera, eventData.cantidad);
			Debug.Log($"[EstadoPanadero] Madera recibida por repartidor: {eventData.cantidad} unidades.");
		}
		else if (eventData.recurso == "harina")
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