using System.Collections;
using UnityEngine;

public abstract class NPCBaseSystems : MonoBehaviour
{
    [Header("NPCBaseSystems")]
    protected bool mirandoPlayer;
    protected bool moviendo;
    public Transform rutaTrazada;
    public Inventario npcInventario;
    protected MoverAgente agente;

    protected virtual void Start()
    {
        agente = GetComponent<MoverAgente>();
        npcInventario = GetComponent<Inventario>();
    }

    protected virtual void Update()
    {
        if (rutaTrazada != null)
            Debug.DrawLine(transform.position, rutaTrazada.position, Color.red);

        if (!moviendo && rutaTrazada != null)
        {
            agente.MoverA(rutaTrazada);
            moviendo = true;
        }

        if (moviendo && agente.HaLlegado())
            moviendo = false;
    }

	public void OnTriggerStay(Collider other)
	{
		// Si el objeto que entra es el jugador, el NPC lo mira
		if (other.CompareTag("Player"))
		{
			mirandoPlayer = true;
			RotateTowardsTarget(other.transform.position);
		}
	}

	private void OnTriggerExit(Collider other)
    {
        mirandoPlayer = false;
    }

    public void RotateTowardsTarget(Vector3 targetPosition)
    {
        Vector3 directionToTarget = targetPosition - transform.position;
        directionToTarget.y = 0;

        if (directionToTarget.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 20f);
        }
    }

    public IEnumerator EsperarConPausa(float tiempo)
    {
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < tiempo)
        {
            if (mirandoPlayer)
            {
                yield return new WaitUntil(() => !mirandoPlayer);
            }

            yield return null;
            tiempoTranscurrido += Time.deltaTime;
        }
    }
}
