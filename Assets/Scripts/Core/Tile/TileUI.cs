using TMPro;
using UnityEngine;

public class TileUI : MonoBehaviour
{
    [SerializeField] private SpriteRenderer icon;
    [SerializeField] private TMP_Text factionText;
    [SerializeField] private TMP_Text layerText;

    [SerializeField] public Sprite[] spriteContainer;

    public void SetIcon(int faction)
    {
        icon.sprite = spriteContainer[faction];
    }

    // public void SetFactionText(int value)
    // {
    //     factionText.text = $"{value}";
    // }

    // public void SetLayerText(int value)
    // {
    //     layerText.text = $"{value}";
    // }
}
