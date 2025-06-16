using UnityEngine;

[CreateAssetMenu(fileName = "NuevoItem", menuName = "Inventario/Item")]
public class ItemData : ScriptableObject
{
	public string nombreItem; // Este campo debe existir
	public Sprite icono;      // (opcional)
}