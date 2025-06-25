using XNode;

[CreateNodeMenu("IA/Accion/Comer")]
public class NodoAccionComer : NodoIA
{
	[Input] public NodoIA entrada;

	public override NodoIA Evaluar(NPC npc)
	{
		npc.Comer();
		return null;
	}
}
