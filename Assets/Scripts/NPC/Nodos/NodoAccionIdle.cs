using XNode;

[CreateNodeMenu("IA/Accion/Idle")]
public class NodoAccionIdle : NodoIA
{
	[Input] public NodoIA entrada;

	public override NodoIA Evaluar(NPC npc)
	{
		npc.EntrarEnIdle();
		return null; // Aqu� pod�s hacer que vuelva al NodoInicio o se quede parado
	}

	public override object GetValue(NodePort port)
	{
		return null;
	}
}
