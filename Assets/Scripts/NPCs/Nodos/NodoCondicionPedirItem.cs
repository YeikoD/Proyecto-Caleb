using UnityEngine;

[CreateNodeMenu("IA/Condiciones/Pedir Item al Soberano")]
public class NodoCondicionPedirItem : NodoCondicionIA
{
	public string idItem = "madera";                 // El item que se desea pedir
	public string claveMemoria = "puedePedirMadera"; // Memoria del NPC: si ya sabe que no puede pedir

	public override NodoIA Ejecutar(NPC npc)
	{
		// Si ya sabe que no puede pedir, se queda piola
		if (npc.Memoria.TryGetValue(claveMemoria, out bool puedePedir) && !puedePedir)
		{
			Debug.Log($"[{npc.Nombre}] Ya sabe que no puede pedir '{idItem}', no insiste.");
			return GetSalidaFalsa();
		}

		// Consulta la voluntad del soberano
		bool permiso = VoluntadDelSoberano.Instancia?.PermiteRetiro(idItem) ?? false;

		if (permiso)
		{
			Debug.Log($"[{npc.Nombre}] Tiene permiso del soberano para pedir '{idItem}'. Puede hablar con Juna.");
			npc.Memoria[claveMemoria] = true;
			return GetSalidaVerdadera();
		}
		else
		{
			Debug.Log($"[{npc.Nombre}] El soberano no permite pedir '{idItem}'. Memoriza para no insistir.");
			npc.Memoria[claveMemoria] = false;
			return GetSalidaFalsa();
		}
	}

	public override string GetDescripcion() => $"¿Tiene permiso para pedir '{idItem}'?";
}
