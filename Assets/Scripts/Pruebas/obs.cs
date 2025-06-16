using UnityEngine;

[System.Serializable]
public class PuntoRuta
{
    public string nombrePunto; // Ej: "Casa", "Taller"
    public Transform destino;  // Asignado desde Inspector
}

[CreateAssetMenu(fileName = "NpcRutas", menuName = "NPC/Ruta")]
public class obs : ScriptableObject
{
    public PuntoRuta[] puntos;
}
