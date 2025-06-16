[System.Serializable]
public class ItemCantidad
{
	public ItemData item;
	public int cantidad;

	public ItemCantidad(ItemData item, int cantidad)
	{
		this.item = item;
		this.cantidad = cantidad;
	}
}