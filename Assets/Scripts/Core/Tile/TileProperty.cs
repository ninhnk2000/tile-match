using UnityEngine;

public class TileProperty : MonoBehaviour
{
    [SerializeField] private int faction;
    [SerializeField] private int layer;

    public int Faction
    {
        get => faction;
        set => faction = value;
    }

    public int Layer
    {
        get => layer;
        set => layer = value;
    }
}
