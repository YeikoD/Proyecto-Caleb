using XNode;

[CreateNodeMenu("IA/Trabajo/Condición - Tiene Tarea")]
public class NodoCondicionTieneTarea : NodoIA
{
	[Input] public NodoIA entrada;
	[Output] public NodoIA siVerdadero;
	[Output] public NodoIA siFalso;

	public override NodoIA Evaluar(NPC npc)
	{
		return npc.tieneTarea
			? GetOutputPort("siVerdadero").Connection?.node as NodoIA
			: GetOutputPort("siFalso").Connection?.node as NodoIA;
	}

	public override object GetValue(NodePort port)
	{
		return null;
	}

}
