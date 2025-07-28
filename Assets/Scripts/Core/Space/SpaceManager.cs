using System.Collections.Generic;
using UnityEngine;

public class SpaceManager : Singleton<SpaceManager>
{
    [SerializeField] private BaseSpace[] spaces;

    public void Fill(BaseTile tile)
    {
        for (int i = 0; i < spaces.Length; i++)
        {
            if (!spaces[i].IsFilled)
            {
                spaces[i].Interact(tile);

                break;
            }
        }
    }

    public void CheckClearTiles()
    {
        Dictionary<int, List<BaseSpace>> matchSpaces = new Dictionary<int, List<BaseSpace>>();

        for (int i = 0; i < spaces.Length; i++)
        {
            if (spaces[i].IsFilled)
            {
                TileProperty tileProperty = spaces[i].Tile.tileServiceLocator.tileProperty;

                if (!matchSpaces.ContainsKey(tileProperty.Faction))
                {
                    matchSpaces.Add(tileProperty.Faction, new List<BaseSpace>() { spaces[i] });
                }
                else
                {
                    matchSpaces[tileProperty.Faction].Add(spaces[i]);
                }
            }
        }

        foreach (var item in matchSpaces)
        {
            if (item.Value.Count >= 3)
            {
                for (int i = 0; i < item.Value.Count; i++)
                {
                    item.Value[i].ClearTile();
                }
            }
        }
    }
}
