using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseDeDatosDeItems", menuName = "Inventario/Base de Datos de Items")]
public class ItemDB : ScriptableObject
{
    public static ItemDB Instancia { get; private set; }

    public List<ItemData> todosLosItems;

    public ItemDB itemDataBase; // Referencia a la base de datos de items

	private void OnEnable()
    {
        // Si ya hay una instancia, no la sobrescribas
        if (Instancia == null)
            Instancia = this;
    }

    // Busca un item por su nombre (por ejemplo: "Hierro")
    public ItemData ObtenerItemPorNombre(string nombre)
    {
        return todosLosItems.Find(item => item.nombreItem == nombre);
    }
}
    