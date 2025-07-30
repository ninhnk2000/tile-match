using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Saferio/IntVariable")]
public class IntVariable : ScriptableObject
{
    [SerializeField] private string saveKey;

    public int Value
    {
        get => DataUtility.Load(saveKey, 1);
        set => DataUtility.Save(saveKey, value);
    }
}
