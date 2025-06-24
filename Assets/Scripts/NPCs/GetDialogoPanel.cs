using UnityEngine;
using TMPro;

public class GetDialogoPanel : MonoBehaviour
{
	[Header("Referencias: DialogoSystems")]
	[SerializeField] private GameObject dialogoPanel;           // Panel del dialogo entero.
	[SerializeField] private TextMeshProUGUI dialogoText;       // Texto del dialogo del NPC.
	[SerializeField] private GameObject buttonPrefab;           // Prefab del boton de opciones.
	[SerializeField] private Transform buttonContent;

}
