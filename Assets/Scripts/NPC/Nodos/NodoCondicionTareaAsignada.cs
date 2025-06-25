using XNode;

[CreateNodeMenu("IA/Condición/TareaAsignada")]
public class NodoCondicionTareaAsignada : NodoIA
{
	[Input] public NodoIA entrada;
	[Output] public NodoIA siVerdadero;
	[Output] public NodoIA siFalso;

	public override NodoIA Evaluar(NPC npc)
	{
		bool tieneTarea = npc.tieneTarea;

		return tieneTarea ?
			GetOutputPort("siVerdadero").Connection?.node as NodoIA :
			GetOutputPort("siFalso").Connection?.node as NodoIA;
	}

	public override object GetValue(NodePort port)
	{
		return null;
	}
}
