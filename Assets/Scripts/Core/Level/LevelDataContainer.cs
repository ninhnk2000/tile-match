using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Saferio/ScrewAway/LevelDataContainer")]
public class LevelDataContainer : ScriptableObject
{
    [SerializeField] private int maxLevel;

    public int MaxLevel
    {
        get => maxLevel;
    }
}