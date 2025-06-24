using TMPro;
using UnityEngine;

public class ReferenciasGlobales : MonoBehaviour
{
	public static ReferenciasGlobales Instancia { get; private set; }

	[Header("Referencias: Scripts")]
	public Inventario inventario;
	public DialogoSystems dialogoSystems;

	[Header("Referencias: InventarioUI")]
	public GameObject panelInventario;
	public TextMeshProUGUI itemCantidadText;

	[Header("Referencias: ItemsDB")]
	private ItemData madera, harina, pan, hierro;

	private void Awake()
	{
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