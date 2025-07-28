using PrimeTween;
using UnityEngine;

public class BaseSpace : MonoBehaviour
{
    [SerializeField] private bool isFilled;

    private BaseTile _tile;

    public bool IsFilled
    {
        get => isFilled;
        set => isFilled = value;
    }

    public BaseTile Tile
    {
        get => _tile;
    }

    public virtual void Interact(BaseTile tile)
    {
        _tile = tile;
    }

    public virtual void ClearTile()
    {
        Tween.Scale(_tile.transform, 0, duration: 0.3f).OnComplete(() =>
        {
            _tile.gameObject.SetActive(false);

            _tile = null;
            isFilled = false;
        });
    }
}
