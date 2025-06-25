using XNode;

[CreateNodeMenu("IA/Trabajo/Finalizar Tarea")]
public class NodoFinalizarTrabajo : NodoIA
{
	[Input] public NodoIA entrada;

	public override NodoIA Evaluar(NPC npc)
	{
		npc.FinalizarTarea();
		return null; // Termina ac�, vuelve al NodoInicio en el pr�ximo ciclo
	}

	public override object GetValue(NodePort port)
	{
		return null;
	}
}
