using XNode;

public abstract class NodoIA : Node
{
	// Este m�todo va a ser ejecutado por el NPC para decidir qu� hacer
	public abstract NodoIA Evaluar(NPC npc);
}
