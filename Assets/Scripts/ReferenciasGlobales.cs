using TMPro;
using UnityEngine;

public class ReferenciasGlobales : MonoBehaviour
{
	public static ReferenciasGlobales Instancia { get; private set; }

	[Header("Referencias: Scripts")]
	public DialogoSystems dialogoSystems;

	[Header("Referencias: InventarioUI")]
	public GameObject panelInventario;
	public TextMeshProUGUI itemCantidadText;

	[Header("Referencias: Almacenes")]
	public PuestoTrabajo PUESTO, PUESTO_HERRERO, PUESTO_PANADERO;

	[Header("Referencias: ItemsDB")]
	private ItemData madera, harina, pan, hierro;

	private void Awake()
	{
		dialogoSystems = GetComponent<DialogoSystems>();

		if (Instancia == null)
		{
			Instancia = this;
			DontDestroyOnLoad(gameObject); // Opcional: Mantener el objeto entre escenas
		}
		else
		{
			Destroy(gameObject);
		}
	}
}