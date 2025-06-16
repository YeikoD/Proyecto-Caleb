using System.Collections.Generic;
using UnityEngine;

public enum TipoMesaTrabajo
{
	Almacen,
	Horno,
	MesaDeCrafteo,
	Yunque
}

public class MesasArtesanos : MonoBehaviour
{
	[SerializeField] private bool isAlmacen;
	[SerializeField] private ItemDB itemDb;                 // Base de datos de items para obtener referencias a los items por nombre.
	[SerializeField] private TipoMesaTrabajo tipoMesaTrabajo; // Tipo de mesa de trabajo (Almacen, Horno, Mesa de Crafteo, Yunque)
	[SerializeField] private NpcOficio npcOficio;

	private ItemData hierro, madera, hierroForjado, harina, masa, pan; // Cachear referencias a los items para evitar búsquedas repetidas.
	private HashSet<Transform> npcsProcesados = new HashSet<Transform>(); // NPCs ya procesados

	private void Start()
	{
		hierro = itemDb.ObtenerItemPorNombre("Hierro");
		madera = itemDb.ObtenerItemPorNombre("Madera");;
		hierroForjado = itemDb.ObtenerItemPorNombre("HierroForjado");
		harina = itemDb.ObtenerItemPorNombre("Harina");
		masa = itemDb.ObtenerItemPorNombre("Masa");
		pan = itemDb.ObtenerItemPorNombre("Pan");

		Debug.Log($"[MesaTrabajo] tipo {tipoMesaTrabajo} inicializada con items: Hierro, madera.");
	}

	private void OnTriggerStay(Collider other)
	{
		if (!other.CompareTag("NPC")) return;

		var ArtesanosSystems = other.GetComponent<ArtesanosSystems>();

		if (ArtesanosSystems != null)
		{
			ArtesanosSystems.RotateTowardsTarget(transform.position); // Hace que el NPC mire la mesa
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("NPC")) return;
		if (npcsProcesados.Contains(other.transform)) return;   // Evita procesar dos veces el mismo NPC

		var ArtesanosSystems = other.GetComponent<ArtesanosSystems>();          // Obtiene el componente ArtesanosSystems del NPC
		var npcInventario = other.GetComponent<Inventario>();       // Obtiene el componente Inventario del NPC

		switch (tipoMesaTrabajo)
		{
			//////////////////// Almacen
			case TipoMesaTrabajo.Almacen:
				var baulinventario = GetComponent<Inventario>();

				// Verifica si el NPC no tiene madera y si hay en el Almacen para enceder el horno
				if (!ArtesanosSystems.hornoEncendido && baulinventario.ObtenerCantidad(madera) > 0 && npcInventario.ObtenerCantidad(madera) < 1)
				{
					baulinventario.QuitarItem(madera, 1);           // Quita 1 madera del inventario del Almacen
					npcInventario.AgregarItem(madera, 1);           // Agrega 1 madera al inventario del NPC

					Debug.Log($"[MesaTrabajo] {other.name} ha tomado madera del Almacen.");
				}

				switch (npcOficio)
				{
					case NpcOficio.Herrero: /// Herrero
						// Tomar hierro del Almacen si el NPC no tiene y hay disponible
						if (ArtesanosSystems.hornoEncendido && baulinventario.ObtenerCantidad(hierro) > 0 && npcInventario.ObtenerCantidad(hierro) < 1)
						{
							baulinventario.QuitarItem(hierro, 1);           // Quita 1 madera del inventario del Almacen
							npcInventario.AgregarItem(hierro, 1);           // Agrega 1 madera al inventario del NPC

							Debug.Log($"[MesaTrabajo] {other.name} ha tomado Hierro del Almacen.");
						}
						// Guardar hierro forjado en el Almacen si el NPC tiene hierro forjado
						if (npcInventario.ObtenerCantidad(hierroForjado) > 0)
						{
							npcInventario.QuitarItem(hierroForjado, 1); // Quita 1 Hierro Forjado del inventario del NPC
							baulinventario.AgregarItem(hierroForjado, 1);// Agrega 1 Hierro Forjado al inventario del Almacen

							Debug.Log($"[MesaTrabajo] {other.name} ha depositado Hierro Forjado en el Almacen.");
						}
						break;
					case NpcOficio.Panadero: /// Panadero
						// Tomar harina del Almacen si el NPC no tiene harina y hay en el Almacen
						if (ArtesanosSystems.hornoEncendido && baulinventario.ObtenerCantidad(harina) > 0 && npcInventario.ObtenerCantidad(harina) < 1)
						{
							baulinventario.QuitarItem(harina, 1); // Quita 1 Harina del inventario del NPC
							npcInventario.AgregarItem(harina, 1);// Agrega 1 Harina al inventario del Almacen
							Debug.Log($"[MesaTrabajo] {other.name} ha obtenido Harina del Almacen.");
						}
						// Guardar pan en el Almacen si el horno está encendido y el NPC tiene pan
						if (ArtesanosSystems.hornoEncendido && npcInventario.ObtenerCantidad(pan) > 0)
						{
							npcInventario.QuitarItem(pan, 1); // Quita 1 Pan del inventario del NPC
							baulinventario.AgregarItem(pan, 1);// Agrega 1 Pan al inventario del Almacen
							Debug.Log($"[MesaTrabajo] {other.name} ha depositado Pan en el Almacen.");
						}
						break;
				}
				break;
			//////////////////// Horno
			case TipoMesaTrabajo.Horno:
				// Verifica si el NPC tiene Hierro y si el horno está encendido
				if (ArtesanosSystems.hornoEncendido)
				{
					switch (npcOficio)
					{
						case NpcOficio.Herrero:
							// Verifica si el NPC tiene hierro y si el horno está encendido
							if (npcInventario.ObtenerCantidad(hierro) > 0)
							{
								npcInventario.QuitarItem(hierro, 1);        // Quita 1 Hierro del inventario del NPC
								npcInventario.AgregarItem(hierroForjado, 1); // Agrega 1 Hierro Forjado al inventario del NPC

								Debug.Log($"[MesaTrabajo] {other.name} Comenzo a fundir Hierro en el horno.");
							}
							break;
						case NpcOficio.Panadero:
							// Verifica si el NPC tiene harina y si el horno está encendido
							if (npcInventario.ObtenerCantidad(masa) > 0)
							{
								npcInventario.QuitarItem(masa, 1);			// Quita 1 Hierro del inventario del NPC
								npcInventario.AgregarItem(pan, 1);			// Agrega 1 Pan al inventario del NPC
								Debug.Log($"[MesaTrabajo] {other.name} Comenzo a hornear masa en el horno.");
							}
							break;
					}
				}
				else
				{
					// Verifica si el NPC tiene madera y si el horno no está encendido
					if (npcInventario.ObtenerCantidad(madera) > 0)
					{
						npcInventario.QuitarItem(madera, 1);    // Quita 1 madera del inventario del NPC
						ArtesanosSystems.hornoEncendido = true;       // Marca que el horno está encendido

						Debug.Log($"[MesaTrabajo] {other.name} ha encendido el horno.");
					}
				}
				break;
			//////////////////// Mesa de Crafteo
			case TipoMesaTrabajo.MesaDeCrafteo:
				switch (npcOficio)
				{
					case NpcOficio.Herrero:
						//...
						break;
					case NpcOficio.Panadero:
						// Transforma Harina en Masa
						if (npcInventario.ObtenerCantidad(harina) > 0)
						{
							npcInventario.QuitarItem(harina, 1);
							npcInventario.AgregarItem(masa, 1); 
						}
						break;
					case NpcOficio.Carpintero:
						//...
						break;
					case NpcOficio.Yunque:
						//...
						break;
				}
				break;
			//////////////////// Yunque
			case TipoMesaTrabajo.Yunque:
				break;
		}

		npcsProcesados.Add(other.transform); // Marca el NPC como procesado
	}

	private void OnTriggerExit(Collider other)
	{
		npcsProcesados.Remove(other.transform); // Limpia el registro al salir
	}
}
