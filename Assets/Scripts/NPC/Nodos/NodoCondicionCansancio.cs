using UnityEngine;
using XNode;

[CreateNodeMenu("IA/Condición/Cansancio")]
public class NodoCondicionCansancio : NodoIA
{
	[Input] public NodoIA entrada;
	[Output] public NodoIA siVerdadero;
	[Output] public NodoIA siFalso;

	[Range(0f, 1f)]
	public float umbralCansancio = 0.7f;

	public override NodoIA Evaluar(NPC npc)
	{
		bool estaCansado = npc.cansancio >= umbralCansancio;

		return estaCansado ?
			GetOutputPort("siVerdadero").Connection?.node as NodoIA :
			GetOutputPort("siFalso").Connection?.node as NodoIA;
	}
	
	public override object GetValue(NodePort port)
	{
		return null;
	}
}
