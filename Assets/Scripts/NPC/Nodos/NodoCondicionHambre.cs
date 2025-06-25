using UnityEngine;
using XNode;

[CreateNodeMenu("IA/Condición/Hambre")]
public class NodoCondicionHambre : NodoIA
{
	[Input] public NodoIA entrada;
	[Output] public NodoIA siVerdadero;
	[Output] public NodoIA siFalso;

	[Range(0f, 1f)]
	public float umbralHambre = 0.7f;


	public override NodoIA Evaluar(NPC npc)
	{
		bool tieneHambre = npc.hambre >= umbralHambre;

		return tieneHambre ?
			GetOutputPort("siVerdadero").Connection?.node as NodoIA :
			GetOutputPort("siFalso").Connection?.node as NodoIA;
	}

	public override object GetValue(NodePort port)
	{
		return null;
	}
}
