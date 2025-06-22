using UnityEngine;

[CreateAssetMenu(fileName = "Nuevo Dialogo", menuName = "Dialogos/Lineas Dialogos")]
public class LineaDialogo : ScriptableObject
{
	[TextArea(2, 5)]
	public string npcText;

	public OpcionDialogo[] opciones;
}
