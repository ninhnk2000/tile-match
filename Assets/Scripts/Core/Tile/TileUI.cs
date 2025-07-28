using TMPro;
using UnityEngine;

public class TileUI : MonoBehaviour
{
    [SerializeField] private SpriteRenderer icon;
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private TMP_Text factionText;
    [SerializeField] private TMP_Text layerText;

    [SerializeField] public Sprite[] spriteContainer;

    public float TileSize => background.GetComponent<SpriteRenderer>().bounds.size.x;

    public void SetIcon(int faction)
    {
        icon.sprite = spriteContainer[faction];
    }

    public void SetLayer(int layer)
    {
        float layerOffset = layer % 2 == 0 ? 0 : 0.5f * TileSize;

        transform.position = new Vector3(transform.position.x + layerOffset, transform.position.y - layerOffset, layer);
    }
}
