using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogoSystems : MonoBehaviour
{
	public static DialogoSystems Instance;

	[Header("UI References")]
	public GameObject dialoguePanel;
	public TextMeshProUGUI dialogueText;

	[Header("Botón dinámico")]
	public GameObject botonPrefab;
	public Transform contenedorBotones;

	public GrafoDialogo grafoActual;
	private NodoDialogo nodoActual;
	private Dictionary<string, bool> worldState = new();

	public Inventario currentInventario;

	// Cachear referencias a items para uso en diálogo
	private ItemData madera, harina, pan, hierro;

	private void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(gameObject);

		dialoguePanel.SetActive(false);

		// Cachear referencias a ítems para no hacer llamadas repetidas
		pan = ItemDB.Instancia.ObtenerItemPorNombre("Pan");
		harina = ItemDB.Instancia.ObtenerItemPorNombre("Harina");
		madera = ItemDB.Instancia.ObtenerItemPorNombre("Madera");
		hierro = ItemDB.Instancia.ObtenerItemPorNombre("Hierro");
	}

	public void IniciarDialogoDesdeGrafo(GrafoDialogo grafo)
	{
		grafoActual = grafo;

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		foreach (var nodo in grafo.nodes)
		{
			if (nodo is NodoDialogo nd && nd.GetInputPort("entrada").Connection == null)
			{
				IniciarDialogo(nd);
				return;
			}
		}

		Debug.LogError("No se encontró nodo inicial en el grafo.");
	}

	void IniciarDialogo(NodoDialogo nodo)
	{
		nodoActual = nodo;
		dialoguePanel.SetActive(true);

		// Reemplazamos variables dinámicas en el texto antes de mostrarlo
		dialogueText.text = ReemplazarVariables(nodo.textoNPC);

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		LimpiarOpciones();

		for (int i = 0; i < nodo.opciones.Length; i++)
		{
			var opcion = nodo.opciones[i];

			if (!CumpleCondiciones(opcion.condiciones))
				continue;

			GameObject nuevoBoton = Instantiate(botonPrefab, contenedorBotones);
			nuevoBoton.GetComponentInChildren<TextMeshProUGUI>().text = opcion.textoJugador;

			int index = i;
			nuevoBoton.GetComponent<Button>().onClick.AddListener(() => ElegirOpcion(index));
		}
	}

	void LimpiarOpciones()
	{
		foreach (Transform hijo in contenedorBotones)
			Destroy(hijo.gameObject);
	}

	void ElegirOpcion(int index)
	{
		var opcion = nodoActual.opciones[index];

		// Aquí se pueden aplicar efectos si existieran

		if (opcion.terminarDialogo)
		{
			EndDialogue();
			return;
		}

		NodoDialogo siguiente = nodoActual.GetSiguienteNodo(index);

		if (siguiente != null)
			IniciarDialogo(siguiente);
		else
			EndDialogue();
	}

	public void SetWorldState(string key, bool value)
	{
		worldState[key] = value;
	}

	bool CumpleCondiciones(CondicionDialogo[] condiciones)
	{
		if (condiciones == null || condiciones.Length == 0) return true;

		foreach (var cond in condiciones)
		{
			if (!worldState.TryGetValue(cond.clave, out bool valor) || valor != cond.valorEsperado)
				return false;
		}
		return true;
	}

	void EndDialogue()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		dialoguePanel.SetActive(false);
		LimpiarOpciones();
		nodoActual = null;
	}

	// Método que reemplaza etiquetas de texto por cantidades dinámicas del inventario actual del NPC
	string ReemplazarVariables(string texto)
	{
		if (currentInventario == null)
			return texto;

		texto = texto.Replace("{madera}", currentInventario.ObtenerCantidad(madera).ToString());
		texto = texto.Replace("{harina}", currentInventario.ObtenerCantidad(harina).ToString());
		texto = texto.Replace("{pan}", currentInventario.ObtenerCantidad(pan).ToString());
		texto = texto.Replace("{hierro}", currentInventario.ObtenerCantidad(hierro).ToString());

		return texto;
	}

	private void OnEnable()
	{
		EventManager.AddListener<Inventario>("EntregarRecursoRepartidor", RecibirInventario);
	}

	private void OnDisable()
	{
		EventManager.RemoveListener<Inventario>("EntregarRecursoRepartidor", RecibirInventario);
	}

	public void RecibirInventario(Inventario inv)
	{
		currentInventario = inv;
	}
}
