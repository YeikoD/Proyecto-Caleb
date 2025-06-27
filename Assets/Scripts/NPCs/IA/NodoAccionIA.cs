using XNode;
using static XNode.Node;

public abstract class NodoAccionIA : NodoIA
{
	[Input(backingValue = ShowBackingValue.Never)] public NodoIA entrada;
	[Output(backingValue = ShowBackingValue.Never)] public NodoIA salida;
}
