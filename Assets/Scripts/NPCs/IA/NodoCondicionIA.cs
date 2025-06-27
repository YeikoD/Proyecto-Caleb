[NodeTint("#0093db")]
public abstract class NodoCondicionIA : NodoIA
{
	[Input(backingValue = ShowBackingValue.Never)] public NodoIA entrada;
	[Output(backingValue = ShowBackingValue.Never)] public NodoIA verdadero;
	[Output(backingValue = ShowBackingValue.Never)] public NodoIA falso;

	protected NodoIA GetSalidaVerdadera()
	{
		var port = GetOutputPort("verdadero");
		return port != null && port.IsConnected ? port.Connection.node as NodoIA : null;
	}

	protected NodoIA GetSalidaFalsa()
	{
		var port = GetOutputPort("falso");
		return port != null && port.IsConnected ? port.Connection.node as NodoIA : null;
	}
}
