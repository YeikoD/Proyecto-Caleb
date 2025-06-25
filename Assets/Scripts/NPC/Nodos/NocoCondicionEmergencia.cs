using XNode;

[CreateNodeMenu("IA/Condición/Emergencia")]
public class NodoCondicionEmergencia : NodoIA
{
	[Input] public NodoIA entrada;
	[Output] public NodoIA siVerdadero;
	[Output] public NodoIA siFalso;

	public override NodoIA Evaluar(NPC npc)
	{
		bool hayEmergencia = npc.tieneEmergencia;

		return hayEmergencia ?
			GetOutputPort("siVerdadero").Connection?.node as NodoIA :
			GetOutputPort("siFalso").Connection?.node as NodoIA;
	}

	public override object GetValue(NodePort port)
	{
		return null;
	}
}
