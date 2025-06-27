using UnityEngine;

public class NPControl : MonoBehaviour
{
	private NPC npcLogica;

	public NodoIA nodoInicial;

	[Header("Asignación inicial (opcional para testeo)")]
	public string nombreNPC;
	public NPCMovimiento moverNpc; // Referencia al componente de movimiento del NPC, si lo hay
	public Inventario Inventario;
	public OficioTipo oficioNPC;

	private void Awake()
	{
		npcLogica = new NPC(nombreNPC, oficioNPC, nodoInicial);
		npcLogica.Controlador = this;

		Inventario = GetComponent<Inventario>();	
	}

	private void Update()
	{
		npcLogica.Pensar();
	}

	public NPC ObtenerNPC()
	{
		return npcLogica;
	}

	public void MoverA(Transform destino)
	{
		if (moverNpc != null)
			moverNpc.MoverA(destino);
	}

	public bool LlegoAlDestino()
	{
		return moverNpc != null && moverNpc.HaLlegado();
	}
}
