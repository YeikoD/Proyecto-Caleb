using System.Collections;
using UnityEngine;

public class EstadoSiervoRepartidor : IOficioState
{
    private OficioSystems npc;

    // Cacheo de referencias a los ítems
    private ItemData madera, harina, pan, hierro;

    // Variables para almacenar los parámetros
    private string solicitante;
    private string recurso;

    public bool repartidorOcupado;

    // Constantes para evitar literales repetidos
    private const string SOLICITUD_HERRERO = "herrero";
    private const string SOLICITUD_PANADERO = "panadero";
    private const string RECURSO_MADERA = "madera";
    private const string RECURSO_HIERRO = "hierro";
    private const string RECURSO_HARINA = "harina";

    public EstadoSiervoRepartidor(OficioSystems npc)
    {
        this.npc = npc;
        // Cachear referencias a los ítems
        pan = ItemDB.Instancia.ObtenerItemPorNombre("Pan");
        harina = ItemDB.Instancia.ObtenerItemPorNombre("Harina");
        madera = ItemDB.Instancia.ObtenerItemPorNombre("Madera");
        hierro = ItemDB.Instancia.ObtenerItemPorNombre("Hierro");
    }

    // Método para configurar los parámetros
    public void ConfigurarParametros(string solicitante, string recurso)
    {
        this.solicitante = solicitante;
        this.recurso = recurso;

        Debug.Log($"[EstadoSiervoRepartidor] Configurando parámetros: solicitante={solicitante}, recurso={recurso}");
    }

    // Método para recibir recursos (si se requiere para el repartidor)
    public void RecibirRecurso((string recurso, int cantidad) eventData)
    {
        Debug.Log($"[EstadoSiervoRepartidor] Evento 'EntregarRecursoRepartidor' recibido con recurso: {eventData.recurso}, cantidad: {eventData.cantidad}");
        // Aquí puedes implementar la lógica de recepción si el repartidor debe almacenar recursos
    }

    public IEnumerator EjecutarRutina()
    {
        yield return npc.EsperarConPausa(15f);
        npc.rutaTrazada = npc.rutasMediodia[0];
        yield return npc.EsperarConPausa(5f);

        if (solicitante == SOLICITUD_HERRERO)
        {
            if (recurso == RECURSO_MADERA && npc.mesa.almacenInv.ObtenerCantidad(madera) > 0)
            {
                yield return EntregarRecurso(madera, 2, "EntregarRecursoHerrero", RECURSO_MADERA);
            }
            else if (recurso == RECURSO_HIERRO && npc.mesa.almacenInv.ObtenerCantidad(hierro) > 0)
            {
                yield return EntregarRecurso(hierro, 2, "EntregarRecursoHerrero", RECURSO_HIERRO);
            }
            else
            {
                npc.rutaTrazada = npc.rutasMediodia[0]; // Esperar
            }
        }
        else if (solicitante == SOLICITUD_PANADERO)
        {
            if (recurso == RECURSO_MADERA && npc.mesa.almacenInv.ObtenerCantidad(madera) > 0)
            {
                yield return EntregarRecurso(madera, 3, "EntregarRecursoPanadero", RECURSO_MADERA);
            }
            else if (recurso == RECURSO_HARINA && npc.mesa.almacenInv.ObtenerCantidad(harina) > 0)
            {
                yield return EntregarRecurso(harina, 3, "EntregarRecursoPanadero", RECURSO_HARINA);
            }
            else
            {
                npc.rutaTrazada = npc.rutasMediodia[0]; // Esperar
            }
        }
    }

    // Método auxiliar para evitar duplicación, pero manteniendo el flujo original
    private IEnumerator EntregarRecurso(ItemData item, int rutaIndex, string evento, string recursoNombre)
    {
        Debug.Log($"[EstadoSiervoRepartidor] Entregando {recursoNombre} al {solicitante}.");

        npc.mesa.almacenInv.QuitarItem(item, 1);
        npc.npcInventario.AgregarItem(item, 1);
        npc.rutaTrazada = npc.rutasMediodia[rutaIndex];

        yield return npc.EsperarConPausa(5f);

        npc.npcInventario.QuitarItem(item, 1);
        EventManager.TriggerEvent(evento, (recursoNombre, 1));
        Debug.Log($"[EstadoSiervoRepartidor] Evento '{evento}' activado con recurso: {recursoNombre}, cantidad: 1");

        yield return npc.EsperarConPausa(5f);

        npc.rutaTrazada = npc.rutasMediodia[0]; // Esperar en el almacen
    }

	private bool _debePausar = false;
	public void MirarJugador(bool mirar)
	{
		_debePausar = mirar;
		Debug.Log($"[EstadoPanadero] Estado de pausa actualizado: {_debePausar}");
	}
}
