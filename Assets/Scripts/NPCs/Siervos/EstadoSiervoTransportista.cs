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

	public IEnumerator EjecutarRutina()
	{
		// Repite la rutina de herrero
		yield return npc.EsperarConPausa(5f);
		npc.CambiarEstado(new EstadoSiervoTransportista(npc));
	}
}
