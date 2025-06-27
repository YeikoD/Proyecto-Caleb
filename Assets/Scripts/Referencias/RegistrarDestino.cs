using UnityEngine;

public class RegistrarDestino : MonoBehaviour
{
	public string id = "puntoX";

	private void Awake()
	{
		if (!string.IsNullOrEmpty(id))
			Referencias.Instancia?.Registrar(id, transform);
		Debug.Log($"Registrado punto '{id}' => {transform.name}");

	}
}
