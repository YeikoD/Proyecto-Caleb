using System.Collections;

public interface ISiervoState
{
	IEnumerator EjecutarRutina(string solicitante, string recurso);
}
