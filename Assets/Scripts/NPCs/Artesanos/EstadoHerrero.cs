using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EstadoHerrero : IOficioState
{
	private OficioSystems npc;

	// Cacheo de referencias a los �tems
	public ItemData hierro, lingoteHierro, madera;

	// Reemplaza los literales por constantes para los recursos
	private const string RECURSO_MADERA = "madera";
	private const string RECURSO_HIERRO = "hierro";

	public EstadoHerrero(OficioSystems npc)
	{
		this.npc = npc;
		// Cachear referencias a los �tems
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
			Debug.Log("[EstadoHerrero] Comprobando los recursos del almac�n");
			npc.rutaTrazada = npc.rutasMediodia[1]; // Almac�n
			yield return npc.EsperarConPausa(5f);

			// Verifica si hay hierro en el inventario del NPC
			if (npc.npcInventario.ObtenerCantidad(hierro) > 0)
			{
				Debug.Log("[EstadoHerrero] Se dirige a fundir el hierro");
				npc.rutaTrazada = npc.rutasMediodia[2]; // Horno
				yield return npc.EsperarConPausa(60f);

				npc.npcInventario.QuitarItem(hierro, 1);
				npc.npcInventario.AgregarItem(lingoteHierro, 1);

				npc.rutaTrazada = npc.rutasMediodia[1]; // Almac�n
				yield return npc.EsperarConPausa(5f);

				if (npc.npcInventario.ObtenerCantidad(lingoteHierro) > 0 && npc.mesa.almacenInv.ObtenerCantidad(lingoteHierro) < 6)
				{
					npc.npcInventario.QuitarItem(lingoteHierro, 1);
					npc.mesa.almacenInv.AgregarItem(lingoteHierro, 1);
					Debug.Log("[EstadoHerrero] Lingote de hierro almacenado.");
				}
			}
			else
			{
				// Si no hay hierro en el inventario del NPC, intenta obtenerlo del almac�n
				if (npc.mesa.almacenInv.ObtenerCantidad(hierro) > 0)
				{
					npc.mesa.almacenInv.QuitarItem(hierro, 1);
					npc.npcInventario.AgregarItem(hierro, 1);
					Debug.Log("[EstadoHerrero] Se ha tomado hierro del almac�n.");
				}
				else
				{
					Debug.Log("[EstadoHerrero] No hay hierro. Solicitando...");
					npc.rutaTrazada = npc.rutasMediodia[5]; // Pedir hierro al soberano
					
					yield return npc.EsperarConPausa(10);

					EventManager.TriggerEvent("InicioTransporte", ("herrero", RECURSO_HIERRO));
					npc.rutaTrazada = npc.rutasMediodia[0]; // Esperar en el taller
					
					yield return npc.EsperarConPausa(25f);
				}
			}
		}
		else
		{
			Debug.Log("[EstadoHerrero] El horno est� apagado. Intentando encenderlo");

			// Intenta encender el horno
			if (npc.npcInventario.ObtenerCantidad(madera) > 0)
			{
				npc.rutaTrazada = npc.rutasMediodia[2]; // Horno
				
				yield return npc.EsperarConPausa(5f);
				
				npc.npcInventario.QuitarItem(madera, 1);
				npc.hornoEncendido = true; // Cambia el estado del horno a encendido
				
				Debug.Log("[EstadoHerrero] Horno encendido. Comenzando a trabajar...");
			}
			else
			{
				Debug.Log("[EstadoHerrero] No hay madera. Buscando en el almac�n...");
				npc.rutaTrazada = npc.rutasMediodia[1]; // Almac�n
				yield return npc.EsperarConPausa(5f);

				// Verifica si hay madera en el almac�n
				if (npc.mesa.almacenInv.ObtenerCantidad(madera) > 0)
				{
					npc.mesa.almacenInv.QuitarItem(madera, 1);
					npc.npcInventario.AgregarItem(madera, 1);

					Debug.Log("[EstadoHerrero] Madera obtenida del almac�n.");
				}
				else
				{
					Debug.Log("[EstadoHerrero] No hay madera en el almac�n.");
				}

				npc.rutaTrazada = npc.rutasMediodia[2]; // Horno
				yield return npc.EsperarConPausa(5f);

				// Intenta encender el horno con la madera obtenida
				if (npc.npcInventario.ObtenerCantidad(madera) > 0)
				{
					npc.npcInventario.QuitarItem(madera, 1);
					npc.hornoEncendido = true; // Cambia el estado del horno a encendido
					
					Debug.Log("[EstadoHerrero] Horno encendido. Comenzando a trabajar...");
				}
				else
				{
					yield return npc.EsperarConPausa(30f);
					
					Debug.Log("[EstadoHerrero] No hay madera para encender el horno. Esperando...");
					npc.rutaTrazada = npc.rutasMediodia[5]; // Pedir madera al soberano
					
					yield return npc.EsperarConPausa(10f);

					EventManager.TriggerEvent("InicioTransporte", ("herrero", RECURSO_MADERA));
					npc.rutaTrazada = npc.rutasMediodia[0]; // Esperar en el taller
					
					yield return npc.EsperarConPausa(25f);
				}
			}
		}

		// Repite la rutina de herrero
		yield return npc.EsperarConPausa(5f);
		npc.CambiarEstado(new EstadoHerrero(npc));
	}

	/// <summary>
	/// Recibe un recurso entregado por el repartidor.
	/// </summary>
	public void RecibirRecurso((string recurso, int cantidad) eventData)
	{
		Debug.Log($"[EstadoHerrero] Evento 'EntregarRecursoHerrero' recibido con recurso: {eventData.recurso}, cantidad: {eventData.cantidad}");

		if (eventData.recurso == "madera")
		{
			npc.mesa.almacenInv.AgregarItem(madera, eventData.cantidad);
			Debug.Log($"[EstadoHerrero] Madera recibida por repartidor: {eventData.cantidad} unidades.");
		}
		else if (eventData.recurso == "hierro")
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