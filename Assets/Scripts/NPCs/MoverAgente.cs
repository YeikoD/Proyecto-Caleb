using UnityEngine;
using UnityEngine.AI;

public class MoverAgente: MonoBehaviour
{
    private NavMeshAgent agente;  // Componente que maneja la navegación y movimiento del NPC.

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();  // Obtenemos el componente NavMeshAgent para mover al NPC.
    }

    // Método para mover al NPC a una posición específica (se le pasa una posición como parámetro)
    public void MoverA(Transform destino)
    {
        if (destino != null)
        {
            agente.SetDestination(destino.position);  // Le decimos al agente que se mueva a la nueva posición.
        }
    }

    // Método para mover al NPC a una posición en el espacio (se le pasa una posición como parámetro)
    public void MoverA(Vector3 destino)
    {
        agente.SetDestination(destino);  // Le decimos al agente que se mueva a la nueva posición.
    }

    // Método que verifica si el NPC ha llegado al destino
    public bool HaLlegado()
    {
        // Devuelve true si el agente ha llegado al destino y ya no está calculando el camino.
        return !agente.pathPending && agente.remainingDistance <= agente.stoppingDistance;
    }
}
