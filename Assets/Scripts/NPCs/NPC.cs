using System.Collections.Generic;
using UnityEngine;

// ======================================================== //
// ====================== NPC LOGICA ====================== //
// ======================================================== //

public enum OficioTipo
{
	Desempleado,
	Herrero,
	Panadero,
	Guardia
}

public class NPC
{
	public string Nombre { get; private set; }
	public NPControl Controlador { get; set; }
	public Inventario Inventario => Controlador?.Inventario;
	public OficioTipo Oficio { get; private set; }

	private GrafoIA grafoActual;
	private NodoIA nodoActual;

	private List<string> tareasPermitidas = new(); // Lista de tareas que el NPC puede realizar según su oficio
	public Dictionary<string, bool> Memoria = new(); // Memoria del NPC para almacenar estados lógicos como "no puedo pedir madera"

	private EstadoHorno horno;

	public NPC(string nombre, OficioTipo oficioInicial, NodoIA nodoInicial)
	{
		Nombre = nombre;
		nodoActual = nodoInicial;
		AsignarOficio(oficioInicial);
		grafoActual = null; // se asigna luego con el nodo correspondiente
	}

	// ===================== NEURONAL ===================== //
	public void AsignarOficio(OficioTipo nuevoOficio)
	{
		Oficio = nuevoOficio;
		tareasPermitidas.Clear();

		switch (Oficio)
		{
			case OficioTipo.Herrero:
				tareasPermitidas.Add("FundirMetal");
				tareasPermitidas.Add("ForjarHerramienta");
				tareasPermitidas.Add("RepararObjeto");
				break;

			case OficioTipo.Panadero:
				tareasPermitidas.Add("HornearPan");
				tareasPermitidas.Add("PrepararMasa");
				tareasPermitidas.Add("CosecharTrigo");
				break;

			case OficioTipo.Guardia:
				tareasPermitidas.Add("Patrullar");
				tareasPermitidas.Add("VigilarEntrada");
				tareasPermitidas.Add("Arrestar");
				break;

			case OficioTipo.Desempleado:
			default:
				break;
		}
	}

	public bool PuedeHacerTarea(string tarea)
	{
		return tareasPermitidas.Contains(tarea);
	}

	public void AsignarSubgrafo(GrafoIA grafo)
	{
		if (grafoActual == grafo) return; // no reasignar el mismo grafo
		grafoActual = grafo;
		// NO asignar nodoActual acá para evitar resetear la ejecución en medio
		Debug.Log($"[{Nombre}] Grafo asignado: {grafo?.name}");
	}

	public void Pensar()
	{
		if (nodoActual != null)
		{
			nodoActual = nodoActual.Ejecutar(this);
			return;
		}

		if (grafoActual == null)
		{
			Debug.LogWarning($"{Nombre} sin grafo asignado.");
			return;
		}

		// Si no hay nodoActual pero sí hay grafo, inicializar el nodoInicial
		nodoActual = grafoActual.nodoInicial;

		if (nodoActual == null)
		{
			Debug.LogWarning($"{Nombre} sin nodoInicial en grafo {grafoActual.name}.");
		}
	}

	// ====================== MEMORIA ====================== //

	public void Olvidar(string clave)
	{
		if (Memoria.ContainsKey(clave))
			Memoria.Remove(clave);
	}

	// ======================= OTROS ======================= //

	// Método para guardar el horno desde un nodo de carga
	public void SetHorno(EstadoHorno nuevoHorno)
	{
		horno = nuevoHorno;
	}

	// Método para que otros nodos (condicionales, acciones) lo consulten
	public EstadoHorno GetHorno()
	{
		return horno;
	}
}
