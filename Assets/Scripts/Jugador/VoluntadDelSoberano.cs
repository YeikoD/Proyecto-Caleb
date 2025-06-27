using System.Collections.Generic;
using UnityEngine;

public class VoluntadDelSoberano : MonoBehaviour
{
	public static VoluntadDelSoberano Instancia { get; private set; }

	// Diccionario de permisos por idItem
	private Dictionary<string, bool> permisosRetiro = new();

	private void Awake()
	{
		if (Instancia != null && Instancia != this)
		{
			Destroy(gameObject);
			return;
		}
		Instancia = this;
		DontDestroyOnLoad(gameObject);

		// Iniciamos con permisos por defecto (ac� pod�s customizar)
		permisosRetiro["madera"] = true;
		permisosRetiro["trigo"] = false;
		permisosRetiro["vinoPuro"] = false;
	}

	/// <summary>
	/// Retorna true si el soberano permite que se retire ese �tem del almac�n.
	/// </summary>
	public bool PermiteRetiro(string idItem)
	{
		return permisosRetiro.TryGetValue(idItem, out bool permitido) && permitido;
	}

	/// <summary>
	/// Cambia la voluntad del soberano para permitir o denegar cierto recurso.
	/// </summary>
	public void EstablecerPermiso(string idItem, bool permitir)
	{
		permisosRetiro[idItem] = permitir;
		Debug.Log($"[VoluntadDelSoberano] Se {(permitir ? "permite" : "proh�be")} retirar '{idItem}'");
		// A futuro: ac� pod�s emitir un evento para que los NPCs escuchen y actualicen su memoria
	}

	/// <summary>
	/// Retorna todos los permisos actuales, �til para UI o debug.
	/// </summary>
	public Dictionary<string, bool> ObtenerTodosLosPermisos() => new(permisosRetiro);
}
