using UnityEngine;

[CreateNodeMenu("IA/Control/Llamar Grafo por Oficio")]
public class NodoLlamarGrafoPorOficio : NodoAccionIA
{
	[Header("Subgrafos por oficio")]
	public GrafoIA grafoHerrero;
	public GrafoIA grafoPanadero;
	public GrafoIA grafoGuardia;

	public override NodoIA Ejecutar(NPC npc)
	{
		switch (npc.Oficio)
		{
			case OficioTipo.Herrero:
				npc.AsignarSubgrafo(grafoHerrero);
				return grafoHerrero != null ? grafoHerrero.nodoInicial : null;

			case OficioTipo.Panadero:
				npc.AsignarSubgrafo(grafoPanadero);
				return grafoPanadero != null ? grafoPanadero.nodoInicial : null;

			case OficioTipo.Guardia:
				npc.AsignarSubgrafo(grafoGuardia);
				return grafoGuardia != null ? grafoGuardia.nodoInicial : null;

			default:
				Debug.LogWarning($"[IA] {npc.Nombre} no tiene subgrafo asignado para el oficio {npc.Oficio}.");
				return null;
		}
	}

	public override string GetDescripcion()
	{
		return "Asigna el subgrafo correcto según el oficio actual del NPC.";
	}
}

