using UnityEngine;
using TMPro;

public class NPCDeciciones : MonoBehaviour
{
	[Header("Referencias: Scripts")]
	[SerializeField] private DialogoSystems dialogoSystems;
	[SerializeField] public OficioSystems oficioSystems;
	[SerializeField] public Inventario inventario;

	[Header("Referencias: Dialogos")]
	public GrafoDialogo grafo;

	private void Start()
	{
		dialogoSystems = ReferenciasGlobales.Instancia.dialogoSystems;
		oficioSystems = GetComponent<OficioSystems>();
		inventario = GetComponent<Inventario>();
	}

	public void	IniciarDialogo()
	{
		dialogoSystems.IniciarDialogoDesdeGrafo(grafo);
	}
}
