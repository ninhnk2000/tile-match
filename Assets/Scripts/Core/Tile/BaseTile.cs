using UnityEngine;

public class BaseTile : MonoBehaviour
{
    public TileServiceLocator tileServiceLocator;

    #region PROPERTY
    public int faction;
    public int layer;
    #endregion

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

        if (layer == 1)
        {
            color = new Color(200 / 255f, 50 / 255f, 80 / 255f, 1);
        }
        if (layer == 2)
        {
            color = new Color(30 / 255f, 150 / 255f, 180 / 255f, 1);
        }

        tileServiceLocator.spriteRenderer.color = color;

        this.layer = layer; 
    }
}
