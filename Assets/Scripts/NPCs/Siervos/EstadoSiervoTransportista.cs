using System.Collections;
using UnityEngine;

public class EstadoSiervoTransportista : ISiervoState
{
	private SiervosSystems npc;

	// Cacheo de referencias a los ítems
	private ItemData madera, harina, masa, pan;

	public EstadoSiervoTransportista(SiervosSystems npc)
	{
		this.npc = npc;
		// Cachear referencias a los ítems
		pan = ItemDB.Instancia.ObtenerItemPorNombre("Pan");
		harina = ItemDB.Instancia.ObtenerItemPorNombre("Harina");
		masa = ItemDB.Instancia.ObtenerItemPorNombre("Masa");
		madera = ItemDB.Instancia.ObtenerItemPorNombre("Madera");
	}

	public IEnumerator EjecutarRutina(string solicitante, string recurso)
	{
		if (solicitante == "herrero")
		{
			yield return npc.EsperarConPausa(15f);
			npc.rutaTrazada = npc.rutasTrabajo[0];
			yield return npc.EsperarConPausa(5f);

			if (recurso == "madera" && npc.mesa.almacenInv.ObtenerCantidad(madera) > 0)
			{
				Debug.Log("[EstadoSiervoTransportista] Entregando madera al herrero.");
				npc.mesa.almacenInv.QuitarItem(madera, 1);
				npc.npcInventario.AgregarItem(madera, 1);
				npc.rutaTrazada = npc.rutasTrabajo[1]; // Taller herrero
				yield return npc.EsperarConPausa(10f);
			}
		}
	}
}
