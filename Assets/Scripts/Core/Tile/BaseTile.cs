using UnityEngine;

public class BaseTile : MonoBehaviour
{
    private TileServiceLocator _tileServiceLocator;

    #region PROPERTY
    public int faction;
    public int layer;
    #endregion

    private void Awake()
    {
        _tileServiceLocator = GetComponent<TileServiceLocator>();

        faction = Random.Range(0, 9);
        layer = Random.Range(0, 3);

        _tileServiceLocator.tileUI.SetFactionText(faction);
    }

    public void SetLayer(int layer)
    {
        int maxLayer = 2;

        transform.GetChild(0).GetComponent<Canvas>().sortingOrder = 2 * layer + 1;

        _tileServiceLocator.spriteRenderer.sortingOrder = 2 * layer;

        float value = 1 - 0.2f * (maxLayer - layer);

        _tileServiceLocator.spriteRenderer.color = new Color(value, value, value, 1);
    }
}
