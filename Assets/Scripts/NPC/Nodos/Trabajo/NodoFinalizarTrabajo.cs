using XNode;

[CreateNodeMenu("IA/Trabajo/Finalizar Tarea")]
public class NodoFinalizarTrabajo : NodoIA
{
	[Input] public NodoIA entrada;

	public override NodoIA Evaluar(NPC npc)
	{
		npc.FinalizarTarea();
		return null; // Termina acá, vuelve al NodoInicio en el próximo ciclo
	}

	public override object GetValue(NodePort port)
	{
		return null;
	}
}
