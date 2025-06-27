using UnityEngine;

[CreateNodeMenu("IA/Acciones/Actualizar Memoria")]
public class NodoAccionActualizarMemoria : NodoAccionIA
{
	public string clave = "puedePedirMadera";
	public bool valor = true;

	public override NodoIA Ejecutar(NPC npc)
	{
		npc.Memoria[clave] = valor;
		Debug.Log($"[{npc.Nombre}] Memoria actualizada: {clave} = {valor}");
		return GetSalida();
	}

	public override string GetDescripcion() => $"Actualizar memoria '{clave}' a {valor}";
}
