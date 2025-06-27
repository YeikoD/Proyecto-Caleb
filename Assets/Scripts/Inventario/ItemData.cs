using UnityEngine;

[CreateAssetMenu(fileName = "NuevoItem", menuName = "Inventario/Item")]
public class ItemData : ScriptableObject
{
	[Header("Datos del Item")]
	public string idItem;         // ID único, ej: "pan", "harina"
	public string nombreItem;     // Nombre visible para el jugador
	public Sprite icono;          // Opcional
}
