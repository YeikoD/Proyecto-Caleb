using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
	private static readonly Dictionary<string, Delegate> events = new Dictionary<string, Delegate>();

	// Registrar un listener
	public static void AddListener<T>(string eventName, Action<T> listener)
	{
		if (!events.ContainsKey(eventName))
		{
			events[eventName] = null; // Crear el evento si no existe
		}
		events[eventName] = (Action<T>)events[eventName] + listener;
	}

	// Eliminar un listener
	public static void RemoveListener<T>(string eventName, Action<T> listener)
	{
		if (events.ContainsKey(eventName))
		{
			events[eventName] = (Action<T>)events[eventName] - listener;
			if (events[eventName] == null) events.Remove(eventName); // Eliminar el evento si no hay más listeners
		}
	}

	// Desencadenar un evento
	public static void TriggerEvent<T>(string eventName, T arg)
	{
		if (events.ContainsKey(eventName) && events[eventName] != null)
		{
			((Action<T>)events[eventName])?.Invoke(arg);
		}
	}
}