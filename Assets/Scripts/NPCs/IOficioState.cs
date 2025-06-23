using System.Collections;

public interface IOficioState
{
	IEnumerator EjecutarRutina();
	void MirarJugador(bool mirar); // Notifica al estado cuando el NPC comienza o deja de mirar al jugador
}