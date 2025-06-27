using System.Collections.Generic;
using UnityEngine;

public class Referencias : MonoBehaviour
{
	public static Referencias Instancia { get; private set; }

	private Dictionary<string, Transform> puntos = new();

	private void Awake()
	{
		if (Instancia != null && Instancia != this)
		{
			Destroy(gameObject);
			return;
		}
		Instancia = this;
		DontDestroyOnLoad(gameObject);
	}

	public void Registrar(string id, Transform punto)
	{
		if (!puntos.ContainsKey(id))
			puntos.Add(id, punto);
		else
			Debug.LogWarning($"Ya existe un punto registrado con el ID '{id}'.");
	}

	public Transform Obtener(string id)
	{
		puntos.TryGetValue(id, out var destino);
		return destino;
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void AsegurarInstancia()
	{
		if (Instancia == null)
		{
			var go = new GameObject("Referencias");
			Instancia = go.AddComponent<Referencias>();
			DontDestroyOnLoad(go);
			Debug.Log("[Referencias] Instancia AUTOCREADA al inicio.");
		}
	}

}
