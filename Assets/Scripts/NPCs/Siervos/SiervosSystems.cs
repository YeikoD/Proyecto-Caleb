using System.Collections;
using UnityEngine;

public enum SiervoOficio
{
    Transportista,
    Domestico
}

public class SiervosSystems : NPCBaseSystems
{
    private ISiervoState estadoActual;
    public SiervoOficio siervoOficio;

	[Header("Rutas de los artesanos")]
    public Transform[] rutasMediodia;

    protected override void Start()
    {
        base.Start();

        switch (siervoOficio)
        {
            case SiervoOficio.Transportista:
                CambiarEstado(new EstadoSiervoTransportista(this));
                break;
                // Otros casos...
        }
    }

    public void CambiarEstado(ISiervoState nuevoEstado)
    {
        if (estadoActual != null)
        {
            StopAllCoroutines();
        }

        estadoActual = nuevoEstado;
        StartCoroutine(estadoActual.EjecutarRutina());
    }
}
