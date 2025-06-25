using System;

[Serializable]
public class OpcionNodo
{
	public string textoJugador;
	public bool terminarDialogo;
	public string condicion;         // En el futuro podemos usar esto para filtrar opciones
	public string valorEsperado;
	public int cambiarRelacion;
	public CondicionDialogo[] condiciones;
}
