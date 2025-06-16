using UnityEngine;

public class InventarioTester : MonoBehaviour
{
	public Inventario inventario;
	public ItemData itemParaProbar;
	public int cantidad = 5;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.X))
		{
			inventario.AgregarItem(itemParaProbar, cantidad);
			Debug.Log($"Se agregaron {cantidad} de {itemParaProbar.nombreItem}");
		}

		if (Input.GetKeyDown(KeyCode.C))
		{
			inventario.QuitarItem(itemParaProbar, cantidad);
			Debug.Log($"Se quitaron {cantidad} de {itemParaProbar.nombreItem}");
		}

		if (Input.GetKeyDown(KeyCode.V))
		{
			inventario.MostrarInventario();
		}
	}
}
