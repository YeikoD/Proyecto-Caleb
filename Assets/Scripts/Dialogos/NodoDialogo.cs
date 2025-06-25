using UnityEngine;
using XNode;

[System.Serializable]
[NodeWidth(500)]
public class NodoDialogo : Node
{
	[Input(backingValue = ShowBackingValue.Never)] public NodoDialogo entrada;

	[TextArea(2, 5)]
	public string textoNPC;

	public OpcionNodo[] opciones;

	[Output(dynamicPortList = true)] public NodoDialogo[] salidas;

	public NodoDialogo GetSiguienteNodo(int index)
	{
		var puerto = GetOutputPort("salidas " + index);
		return puerto != null ? puerto.Connection?.node as NodoDialogo : null;
	}
}
