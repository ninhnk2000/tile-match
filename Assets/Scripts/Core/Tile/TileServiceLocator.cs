using UnityEngine;

public class TileServiceLocator : MonoBehaviour
{
    [SerializeField] public BaseTile tile;
    [SerializeField] public TileUI tileUI;
    [SerializeField] public TileMaterialPropertyBlock tileMaterialPropertyBlock;

    #region BASIC COMPONENT
    [SerializeField] public SpriteRenderer spriteRenderer;
    #endregion
}
