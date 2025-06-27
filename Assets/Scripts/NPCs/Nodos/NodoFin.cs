[NodeTint("#E6484C")]
[CreateNodeMenu("IA/Acciones/Fin")]
public class NodoFin : NodoIA
{
	[Input(backingValue = ShowBackingValue.Never)] public NodoIA entrada;

	public override NodoIA Ejecutar(NPC npc)
	{
		// Acá no hacemos nada, solo indicamos fin retornando null
		return null;
	}

	public override string GetDescripcion() => "Fin de la rutina";
}
