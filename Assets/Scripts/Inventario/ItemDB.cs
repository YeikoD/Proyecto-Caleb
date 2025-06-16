using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseDeDatosDeItems", menuName = "Inventario/Base de Datos de Items")]
public class ItemDB : ScriptableObject
{
	public List<ItemData> todosLosItems;

	// Busca un item por su nombre (por ejemplo: "Hierro")
	public ItemData ObtenerItemPorNombre(string nombre)
	{
		return todosLosItems.Find(item => item.nombreItem == nombre);
	}
}
