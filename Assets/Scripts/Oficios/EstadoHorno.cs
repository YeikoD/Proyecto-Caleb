using UnityEngine;

public class EstadoHorno : MonoBehaviour
{
	public bool encendido = false;

	public bool EstaEncendido()
	{
		return encendido;
	}

	public void Encender()
	{
		encendido = true;
		Debug.Log("[Horno] ¡Encendido!");
	}

	public void Apagar()
	{
		encendido = false;
		Debug.Log("[Horno] Apagado.");
	}
}

