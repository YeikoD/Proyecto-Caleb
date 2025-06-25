using XNode;

[CreateNodeMenu("IA/Trabajo/Acci�n - Ejecutar Tarea")]
public class NodoAccionEjecutarTarea : NodoIA
{
	[Input] public NodoIA entrada;

	public override NodoIA Evaluar(NPC npc)
	{
		npc.EjecutarTarea();
		return GetOutputPort("salida").Connection?.node as NodoIA;
	}

	[Output] public NodoIA salida;

	public override object GetValue(NodePort port)
	{
		return null;
	}

}
