using UnityEngine;
using UnityEngine.AI;

public class MoverAgente: MonoBehaviour
{
    private NavMeshAgent agente;  // Componente que maneja la navegaci�n y movimiento del NPC.

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();  // Obtenemos el componente NavMeshAgent para mover al NPC.
    }

    // M�todo para mover al NPC a una posici�n espec�fica (se le pasa una posici�n como par�metro)
    public void MoverA(Transform destino)
    {
        if (destino != null)
        {
            agente.SetDestination(destino.position);  // Le decimos al agente que se mueva a la nueva posici�n.
        }
    }

    // M�todo para mover al NPC a una posici�n en el espacio (se le pasa una posici�n como par�metro)
    public void MoverA(Vector3 destino)
    {
        agente.SetDestination(destino);  // Le decimos al agente que se mueva a la nueva posici�n.
    }

    // M�todo que verifica si el NPC ha llegado al destino
    public bool HaLlegado()
    {
        // Devuelve true si el agente ha llegado al destino y ya no est� calculando el camino.
        return !agente.pathPending && agente.remainingDistance <= agente.stoppingDistance;
    }
}
