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
	private LineaDialogo lineaActual;
	private Dictionary<string, bool> worldState = new();

	private void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(gameObject);

		dialoguePanel.SetActive(false);
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
		dialogueText.text = nodo.textoNPC;

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		LimpiarOpciones();

		for (int i = 0; i < nodo.opciones.Length; i++)
		{
			var opcion = nodo.opciones[i];
			GameObject nuevoBoton = Instantiate(botonPrefab, contenedorBotones);
			nuevoBoton.GetComponentInChildren<TextMeshProUGUI>().text = opcion.textoJugador;

			int index = i;
			nuevoBoton.GetComponent<Button>().onClick.AddListener(() => ElegirOpcion(index));
		}
	}

	void ShowLine(LineaDialogo line)
	{
		lineaActual = line;
		dialogueText.text = line.npcText;

		LimpiarOpciones();

		for (int i = 0; i < line.opciones.Length; i++)
		{
			var option = line.opciones[i];

			bool show = true;
			if (!string.IsNullOrEmpty(option.llaveCondicion))
			{
				show = worldState.TryGetValue(option.llaveCondicion, out bool value) && value == option.requiredValue;
			}

			if (show)
			{
				GameObject nuevoBoton = Instantiate(botonPrefab, contenedorBotones);
				nuevoBoton.GetComponentInChildren<TextMeshProUGUI>().text = option.textoJugador;

				int index = i;
				nuevoBoton.GetComponent<Button>().onClick.AddListener(() => SelectOption(index));
			}
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

		if (opcion.cambiarRelacion != 0)
			Debug.Log($"Relación modificada en {opcion.cambiarRelacion}");

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

	void SelectOption(int index)
	{
		var selected = lineaActual.opciones[index];

		foreach (var efecto in selected.efectos)
			efecto?.EjecutarEfecto(null, null); // Por ahora null

		if (selected.terminarDialogo || selected.siguienteLinea == null)
			EndDialogue();
		else
			ShowLine(selected.siguienteLinea);
	}

	public void SetWorldState(string key, bool value)
	{
		worldState[key] = value;
	}

	void EndDialogue()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		dialoguePanel.SetActive(false);
		lineaActual = null;
		LimpiarOpciones();
	}
}
