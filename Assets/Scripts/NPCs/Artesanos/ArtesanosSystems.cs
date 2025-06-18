using System.Collections;
using UnityEngine;

public enum Oficio
{
    Herrero,
    Panadero
}

public class ArtesanosSystems : NPCBaseSystems
{
    private IOficioState estadoActual;

	[Header("Info")]
	public bool hornoEncendido; // Indica si el horno está encendido o no
	public Oficio oficio;

	[Header("Referencias")]
	public MesaInteraccion mesa; // Inventario de alamcenamiento

	[Header("Rutas de los artesanos")]
    public Transform[] rutasMediodia;

    protected override void Start()
    {
        base.Start();
        hornoEncendido = false; // Por defecto el horno está apagado

        switch (oficio)
        {
            case Oficio.Herrero:
                CambiarEstado(new EstadoHerrero(this));
                break;
            case Oficio.Panadero:
                CambiarEstado(new EstadoPanadero(this));
                break;
                // Otros casos...
        }
    }

    public void CambiarEstado(IOficioState nuevoEstado)
    {
        if (estadoActual != null)
        {
            StopAllCoroutines();
        }

        estadoActual = nuevoEstado;
        StartCoroutine(estadoActual.EjecutarRutina());
    }
}
