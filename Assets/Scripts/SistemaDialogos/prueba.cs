using UnityEngine;

public class prueba : MonoBehaviour
{
	public GrafoDialogo miGrafo;

	public void IniciarConversacion()
	{
		DialogoSystems.Instance.IniciarDialogoDesdeGrafo(miGrafo);
	}
}
