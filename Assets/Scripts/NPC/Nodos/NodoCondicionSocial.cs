using UnityEngine;
using XNode;

[CreateNodeMenu("IA/Condición/Social")]
public class NodoCondicionSocial : NodoIA
{
	[Input] public NodoIA entrada;
	[Output] public NodoIA siVerdadero;
	[Output] public NodoIA siFalso;

	[Range(0f, 1f)]
	public float umbralSociabilidad = 0.5f;

	public override NodoIA Evaluar(NPC npc)
	{
		bool quiereSocializar = npc.sociabilidad >= umbralSociabilidad;

		return quiereSocializar ?
			GetOutputPort("siVerdadero").Connection?.node as NodoIA :
			GetOutputPort("siFalso").Connection?.node as NodoIA;
	}

	public override object GetValue(NodePort port)
	{
		return null;
	}
}
