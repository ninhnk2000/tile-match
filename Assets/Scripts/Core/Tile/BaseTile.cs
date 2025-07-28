using UnityEngine;

public class BaseTile : MonoBehaviour
{
    public TileServiceLocator tileServiceLocator;

    // #region PROPERTY
    // public int faction;
    // public int layer;
    // #endregion

    private void Awake()
    {
        tileServiceLocator = GetComponent<TileServiceLocator>();

        // faction = Random.Range(0, 9);
        // layer = Random.Range(0, 3);

        // tileServiceLocator.tileUI.SetFactionText(faction);
    }

    public void SetLayer(int layer)
    {
        int maxLayer = 2;

        transform.GetChild(0).GetComponent<Canvas>().sortingOrder = 2 * layer + 1;

        tileServiceLocator.spriteRenderer.sortingOrder = 2 * layer;

        float value = 1 - 0.2f * (maxLayer - layer);

        Color color = Color.white;

        color = ((float)layer / maxLayer) * new Color(0.3f, 0.3f, 0.3f, 1);

        // tileServiceLocator.spriteRenderer.color = color;
        tileServiceLocator.tileProperty.Layer = layer;
        tileServiceLocator.tileUI.SetLayer(layer);
    }
}
