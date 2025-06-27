using static XNode.Node;
using XNode;

public abstract class NodoIA : Node
{
	//[Input(backingValue = ShowBackingValue.Never)] public NodoIA entrada;
	//[Output(backingValue = ShowBackingValue.Never)] public NodoIA salida;

	/// <summary>
	/// Devuelve el siguiente nodo si terminó, o `this` si quiere seguir ejecutándose en el próximo frame.
	/// </summary>
	public abstract NodoIA Ejecutar(NPC npc);

	public virtual string GetDescripcion() => name;

	protected NodoIA GetSalida()
	{
		var port = GetOutputPort("salida");
		if (port != null && port.IsConnected)
			return port.Connection.node as NodoIA;
		return null;
	}

}

