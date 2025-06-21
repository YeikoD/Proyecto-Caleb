using System.Collections;
using UnityEngine;

public class EstadoSiervoRepartidor : IOficioState
{
	private OficioSystems npc;

	// Cacheo de referencias a los ítems
	public ItemData madera, harina, masa, pan, hierro;

	// Variables para almacenar los parámetros
	private string solicitante;
	private string recurso;

	public bool repartidorOcupado;

	public EstadoSiervoRepartidor(OficioSystems npc)
	{
		this.npc = npc;
		// Cachear referencias a los ítems
		pan = ItemDB.Instancia.ObtenerItemPorNombre("Pan");
		harina = ItemDB.Instancia.ObtenerItemPorNombre("Harina");
		masa = ItemDB.Instancia.ObtenerItemPorNombre("Masa");
		madera = ItemDB.Instancia.ObtenerItemPorNombre("Madera");
		hierro = ItemDB.Instancia.ObtenerItemPorNombre("Hierro");
	}

	// Método para configurar los parámetros
	public void ConfigurarParametros(string solicitante, string recurso)
	{
		this.solicitante = solicitante;
		this.recurso = recurso;

		Debug.Log($"[EstadoSiervoRepartidor] Configurando parámetros: solicitante={solicitante}, recurso={recurso}");
	}

	// Método para recibir recursos (si se requiere para el repartidor)
	public void RecibirRecurso((string recurso, int cantidad) eventData)
	{
		Debug.Log($"[EstadoSiervoRepartidor] Evento 'EntregarRecursoRepartidor' recibido con recurso: {eventData.recurso}, cantidad: {eventData.cantidad}");
		// Aquí puedes implementar la lógica de recepción si el repartidor debe almacenar recursos
	}

	// Implementación obligatoria pero ignorada
	public IEnumerator EjecutarRutina()
	{
		// Usar los parámetros configurados
		if (solicitante == "herrero")
		{
			yield return npc.EsperarConPausa(15f);
			npc.rutaTrazada = npc.rutasMediodia[1];
			yield return npc.EsperarConPausa(5f);

			// Verifica si el herrero nesesita madera
			if (recurso == "madera" && npc.mesa.almacenInv.ObtenerCantidad(madera) > 0)
			{
				Debug.Log("[EstadoSiervoRepartidor] Entregando madera al herrero.");

				npc.mesa.almacenInv.QuitarItem(madera, 1);
				npc.npcInventario.AgregarItem(madera, 1);
				npc.rutaTrazada = npc.rutasMediodia[2];

				yield return npc.EsperarConPausa(5f);

				npc.npcInventario.QuitarItem(madera, 1);
				EventManager.TriggerEvent("EntregarRecursoHerrero", ("madera", 1));
				Debug.Log("[EstadoSiervoRepartidor] Evento 'EntregarRecursoHerrero' activado con recurso: madera, cantidad: 1");

				yield return npc.EsperarConPausa(5f);

				npc.rutaTrazada = npc.rutasMediodia[0]; // Esperar en el almacen
			}
			
			// Verifica si el herrero nesesita hierro
			else if (recurso == "hierro" && npc.mesa.almacenInv.ObtenerCantidad(hierro) > 0)
			{
				Debug.Log("[EstadoSiervoRepartidor] Entregando hierro al herrero.");
				npc.mesa.almacenInv.QuitarItem(hierro, 1);
				npc.npcInventario.AgregarItem(hierro, 1);
				npc.rutaTrazada = npc.rutasMediodia[2];
				yield return npc.EsperarConPausa(5f);
				
				npc.npcInventario.QuitarItem(hierro, 1);
				EventManager.TriggerEvent("EntregarRecursoHerrero", ("hierro", 1));
				Debug.Log("[EstadoSiervoRepartidor] Evento 'EntregarRecursoHerrero' activado con recurso: hierro, cantidad: 1");
				
				yield return npc.EsperarConPausa(5f);
				
				npc.rutaTrazada = npc.rutasMediodia[0]; // Esperar en el almacen
			}
			else
			{
				npc.rutaTrazada = npc.rutasMediodia[0]; // Esperar
			}
		}
		else if (solicitante == "panadero")
		{
			yield return npc.EsperarConPausa(15f);
			npc.rutaTrazada = npc.rutasMediodia[1];
			yield return npc.EsperarConPausa(5f);

			// Verifica si el panadero nesesita madera
			if (recurso == "madera" && npc.mesa.almacenInv.ObtenerCantidad(madera) > 0)
			{
				Debug.Log("[EstadoSiervoRepartidor] Entregando madera al panadero.");

				npc.mesa.almacenInv.QuitarItem(madera, 1);
				npc.npcInventario.AgregarItem(madera, 1);
				npc.rutaTrazada = npc.rutasMediodia[3];

				yield return npc.EsperarConPausa(5f);

				npc.npcInventario.QuitarItem(madera, 1);
				EventManager.TriggerEvent("EntregarRecursoPanadero", ("madera", 1));
				Debug.Log("[EstadoSiervoRepartidor] Evento 'EntregarRecursoPanadero' activado con recurso: madera, cantidad: 1");

				yield return npc.EsperarConPausa(5f);

				npc.rutaTrazada = npc.rutasMediodia[0]; // Esperar en el almacen
			}

			// Verifica si el panadero nesesita harina
			else if (recurso == "harina" && npc.mesa.almacenInv.ObtenerCantidad(harina) > 0)
			{
				Debug.Log("[EstadoSiervoRepartidor] Entregando harina al panadero.");

				npc.mesa.almacenInv.QuitarItem(harina, 1);
				npc.npcInventario.AgregarItem(harina, 1);
				npc.rutaTrazada = npc.rutasMediodia[3];

				yield return npc.EsperarConPausa(5f);

				npc.npcInventario.QuitarItem(harina, 1);
				EventManager.TriggerEvent("EntregarRecursoPanadero", ("harina", 1));
				Debug.Log("[EstadoSiervoRepartidor] Evento 'EntregarRecursoPanadero' activado con recurso: harina, cantidad: 1");

				yield return npc.EsperarConPausa(5f);

				npc.rutaTrazada = npc.rutasMediodia[0]; // Esperar en el almacen
			}
			else
			{
				npc.rutaTrazada = npc.rutasMediodia[0]; // Esperar
			}
		}
	}
}
