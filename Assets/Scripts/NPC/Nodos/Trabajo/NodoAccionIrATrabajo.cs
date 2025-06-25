using XNode;

[CreateNodeMenu("IA/Trabajo/Acción - Ir al Trabajo")]
public class NodoAccionIrATrabajo : NodoIA
{
	[Input] public NodoIA entrada;

	public override NodoIA Evaluar(NPC npc)
	{
		npc.IrAlTrabajo();
		return GetOutputPort("salida").Connection?.node as NodoIA;
	}

	[Output] public NodoIA salida;

	public override object GetValue(NodePort port)
	{
		return null;
	}

}
