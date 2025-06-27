using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseDeDatosDeItems", menuName = "Inventario/Base de Datos de Items")]
public class ItemDB : ScriptableObject
{
	public static ItemDB Instancia { get; private set; }

	public List<ItemData> todosLosItems;

	private void OnEnable()
	{
		if (Instancia == null)
			Instancia = this;
	}

	public ItemData ObtenerItemPorNombre(string nombre)
	{
		return todosLosItems.Find(item => item.nombreItem == nombre);
	}

	public ItemData ObtenerPorID(string id)
	{
		return todosLosItems.Find(item => item.idItem == id);
	}
}
