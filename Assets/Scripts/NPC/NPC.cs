using UnityEngine;

public enum TipoEmergencia
{
	Ninguna,
	Enemigo,
	Incendio,
	AtaqueBestia,
	Envenenamiento
}

public class NPC
{
	public string nombre = "JuanNPC";
	public NPCMovimiento cuerpo;

	// === ESTADO BÁSICO ===
	public float hambre = 0.5f; // 0 (lleno) - 1 (muerto de hambre)
	public float cansancio = 0.2f; // 0 (descansado) - 1 (hecho bolsa)
	public float sociabilidad = 0.6f; // 0 (antisocial) - 1 (necesita mimos)

	// === TAREA Y ACCIONES ===
	public bool tieneTarea = false;

	// === EMERGENCIAS ===
	public TipoEmergencia tipoEmergencia = TipoEmergencia.Ninguna;
	public bool tieneEmergencia => tipoEmergencia != TipoEmergencia.Ninguna;

	// === TRABAJOS ===
	public Transform puntoDeTrabajo;

	// === MÉTODOS DE ACCIÓN ===
	public void Huir()
	{
		Debug.Log(nombre + " está huyendo de la emergencia.");
	}

	public void PedirAyuda()
	{
		Debug.Log(nombre + " está pidiendo ayuda.");
	}

	public void Esconderse()
	{
		Debug.Log(nombre + " está escondiéndose.");
	}

	public void Defenderse()
	{
		Debug.Log(nombre + " se está defendiendo.");
	}
	
	// Chill
	public void Comer()
	{
		Debug.Log(nombre + " está comiendo... Hambre antes: " + hambre);
		hambre = Mathf.Max(0f, hambre - 0.5f);
		Debug.Log("Hambre después: " + hambre);
	}

	public void Dormir()
	{
		Debug.Log(nombre + " está descansando.");
		cansancio = Mathf.Max(0f, cansancio - 0.5f);
	}

	public void Socializar()
	{
		Debug.Log(nombre + " está hablando con alguien.");
		sociabilidad = Mathf.Max(0f, sociabilidad - 0.3f);
	}

	public void EntrarEnIdle()
	{
		Debug.Log(nombre + " está sin hacer nada. (Idle)");
	}

	// Trabajo
	public void IrAlTrabajo()
	{
		Debug.Log(nombre + " se dirige al trabajo.");
	}

	public void EjecutarTarea()
	{
		Debug.Log(nombre + " está trabajando.");
		// Acá podés llamar a una función del sistema de siembra, herrería, etc.
	}

	public void FinalizarTarea()
	{
		Debug.Log(nombre + " terminó su tarea.");
		tieneTarea = false;
	}
}
