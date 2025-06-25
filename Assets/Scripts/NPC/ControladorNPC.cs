using System.Linq;
using UnityEngine;

public class ControladorNPC : MonoBehaviour
{
	public GrafoIA grafo; // Asignalo desde el Inspector
	private NodoIA nodoActual;
	private NPC npc;

	[Header("Evaluación Automática")]
	public float tiempoEntrePensamientos = 5f; // Cada cuánto evalúa el grafo
	private float tiempoSiguienteEvaluacion;

	void Start()
	{
		npc = new NPC();
		npc.nombre = "Pepe";
		npc.hambre = Random.Range(0f, 1f);

		// 🔌 Conexión importante
		npc.cuerpo = GetComponent<NPCMovimiento>(); // Conectamos el cuerpo físico

		// 🔧 (opcional) si ya sabés el punto de trabajo
		// npc.puntoDeTrabajo = GameObject.Find("Puesto_Herrero").transform;

		nodoActual = grafo.nodes.OfType<NodoInicio>().FirstOrDefault();

		if (nodoActual == null)
		{
			Debug.LogError("No se encontró un NodoInicio en el grafo asignado.");
		}

		tiempoSiguienteEvaluacion = Time.time + tiempoEntrePensamientos;
	}

	void Update()
	{
		if (Time.time >= tiempoSiguienteEvaluacion && nodoActual != null)
		{
			nodoActual = nodoActual.Evaluar(npc);
			tiempoSiguienteEvaluacion = Time.time + tiempoEntrePensamientos;
		}
	}
}
