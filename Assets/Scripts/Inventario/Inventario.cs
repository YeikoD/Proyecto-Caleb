using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour
{
	[SerializeField] private ItemDB itemDb;
	[SerializeField] private List<ItemCantidad> inventario = new List<ItemCantidad>();

	private void Awake()
	{
		if (itemDb != null)
			InicializarDesdeBaseDeDatos(itemDb);

		itemDb = ItemDB.Instancia.itemDataBase; // Asigna la base de datos de items desde el singleton
	}

	// Agrega una cantidad de un item al inventario. Si el item ya existe, suma la cantidad; si no, lo añade.
	public void AgregarItem(ItemData item, int cantidad)
	{
		ItemCantidad existente = inventario.Find(x => x.item == item);
		if (existente != null)
		{
			existente.cantidad += cantidad;
		}
		else
		{
			inventario.Add(new ItemCantidad(item, cantidad));
		}
	}

	// Quita una cantidad de un item del inventario si hay suficientes. Elimina el item si la cantidad llega a cero.
	public bool QuitarItem(ItemData item, int cantidad)
	{
		ItemCantidad existente = inventario.Find(x => x.item == item);
		if (existente != null && existente.cantidad >= cantidad)
		{
			existente.cantidad -= cantidad;
			if (existente.cantidad == 0)
				inventario.Remove(existente);
			return true;
		}
		return false;
	}

	// Devuelve la cantidad actual de un item en el inventario.
	public int ObtenerCantidad(ItemData item)
	{
		ItemCantidad existente = inventario.Find(x => x.item == item);
		return existente != null ? existente.cantidad : 0;
	}

	// Muestra el inventario actual en la consola (para pruebas).
	public void MostrarInventario()
	{
		Debug.Log($"Inventario de {gameObject.name}:");
		foreach (ItemCantidad ic in inventario)
		{
			Debug.Log($"- {ic.item.nombreItem}: {ic.cantidad}");
		}
	}

	// Inicializa el inventario con todos los items de una base de datos, en cantidad cero.
	public void InicializarDesdeBaseDeDatos(ItemDB bd)
	{
		//inventario.Clear();
		foreach (var item in bd.todosLosItems)
		{
			inventario.Add(new ItemCantidad(item, 0));
		}
	}
}
