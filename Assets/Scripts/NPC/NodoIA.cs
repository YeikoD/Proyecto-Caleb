using XNode;

public abstract class NodoIA : Node
{
	// Este método va a ser ejecutado por el NPC para decidir qué hacer
	public abstract NodoIA Evaluar(NPC npc);
}
