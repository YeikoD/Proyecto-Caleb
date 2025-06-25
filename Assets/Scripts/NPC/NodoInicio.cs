using XNode;

[CreateNodeMenu("IA/Inicio")]
public class NodoInicio : NodoIA
{
	[Output] public NodoIA siguiente;

	public override NodoIA Evaluar(NPC npc)
	{
		return GetOutputPort("siguiente").Connection?.node as NodoIA;
	}
}