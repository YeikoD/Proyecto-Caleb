using UnityEngine;
using UnityEngine.AI;

public class NPCMovimiento : MonoBehaviour
{
	private NavMeshAgent agente;

	private void Awake()
	{
		agente = GetComponent<NavMeshAgent>();
	}

	public void MoverA(Transform destino)
	{
		if (destino != null)
			agente.SetDestination(destino.position);
	}

	public void MoverA(Vector3 destino)
	{
		agente.SetDestination(destino);
	}

	public bool HaLlegado(float tolerancia = 0.3f)
	{
		return !agente.pathPending && agente.remainingDistance <= tolerancia;
	}

	public void DetenerMovimiento(bool pausar)
	{
		agente.isStopped = pausar;
	}
}
