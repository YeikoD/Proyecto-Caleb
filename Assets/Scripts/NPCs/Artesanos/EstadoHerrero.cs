using System.Collections;
using UnityEngine;

public class EstadoHerrero : IOficioState
{
	private ArtesanosSystems npc;

	// Cacheo de referencias a los ítems
	private ItemData hierro;
	private ItemData lingoteHierro;
	private ItemData madera;

	public EstadoHerrero(ArtesanosSystems npc)
	{
		this.npc = npc;
		// Cachear referencias a los ítems
		hierro = ItemDB.Instancia.ObtenerItemPorNombre("Hierro");
		lingoteHierro = ItemDB.Instancia.ObtenerItemPorNombre("LingoteHierro");
		madera = ItemDB.Instancia.ObtenerItemPorNombre("Madera");
	}

	public IEnumerator EjecutarRutina()
	{
		Debug.Log("[EstadoHerrero] Re / Iniciando rutina...");

		if (npc.hornoEncendido)
		{
			Debug.Log("[EstadoHerrero] Comprobando los recursos del almacén");
			npc.rutaTrazada = npc.rutasMediodia[1]; // Almacén
			yield return npc.EsperarConPausa(5f);

			if (npc.npcInventario.ObtenerCantidad(hierro) > 0)
			{
				Debug.Log("[EstadoHerrero] Se dirige a fundir el hierro");
				npc.rutaTrazada = npc.rutasMediodia[2]; // Horno
				yield return npc.EsperarConPausa(60f);

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
			else
			{
				if (npc.mesa.almacenInv.ObtenerCantidad(hierro) > 0)
				{
					npc.mesa.almacenInv.QuitarItem(hierro, 1);
					npc.npcInventario.AgregarItem(hierro, 1);
					Debug.Log("[EstadoHerrero] Se ha tomado hierro del almacén.");
				}
			}
		}
		else
		{
			Debug.Log("[EstadoHerrero] El horno está apagado. Intentando encenderlo");

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
				Debug.Log("[EstadoHerrero] No hay madera. Buscando en el almacén...");
				npc.rutaTrazada = npc.rutasMediodia[1]; // Almacén
				yield return npc.EsperarConPausa(5f);

				// Verifica si hay madera en el almacén
				if (npc.mesa.almacenInv.ObtenerCantidad(madera) > 0)
				{
					npc.mesa.almacenInv.QuitarItem(madera, 1);
					npc.npcInventario.AgregarItem(madera, 1);

					Debug.Log("[EstadoHerrero] Madera obtenida del almacén.");
				}
				else
				{
					Debug.Log("[EstadoHerrero] No hay madera en el almacén.");
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
					Debug.Log("[EstadoHerrero] No hay madera para encender el horno. Esperando...");
					npc.rutaTrazada = npc.rutasMediodia[5]; // Pedir madera al soberano
					yield return npc.EsperarConPausa(30f);
					npc.rutaTrazada = npc.rutasMediodia[0]; // Esperar en el taller
					yield return npc.EsperarConPausa(30f);
				}
			}
		}

		// Repite la rutina de herrero
		yield return npc.EsperarConPausa(5f);
		npc.CambiarEstado(new EstadoHerrero(npc));
	}
}