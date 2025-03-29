using TMPro;
using UnityEngine;

public class TileUI : MonoBehaviour
{
    [SerializeField] private TMP_Text factionText;
    [SerializeField] private TMP_Text layerText;

    public void SetFactionText(int value)
    {
        factionText.text = $"{value}";
    }

    public void SetLayerText(int value)
    {
        layerText.text = $"{value}";
    }
}
