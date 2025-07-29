using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private BaseTile tilePrefab;

    #region PRIVATE FIELD
    private List<BaseTile> _spawnedTiles;
    #endregion

    private void Awake()
    {
        GenerateLayer(0);
        // GenerateBoard();
    }

    private void GenerateBoard()
    {
        int row = 3;
        int column = 2;

        float tileSize = tilePrefab.GetComponent<TileUI>().TileSize;

        Vector2 offset = Vector2.zero;

        Dictionary<Vector2, SpawnTileInfo> checkList = new Dictionary<Vector2, SpawnTileInfo>();

        _spawnedTiles = new List<BaseTile>();

        for (int i = 0; i <= row; i++)
        {
            for (int j = 0; j <= column; j++)
            {
                bool isRandomSpawn = GetRandomPercentChance(75);

                if (isRandomSpawn)
                {
                    // checkList.Add(new Vector2(j, i), true);
                }
                else
                {
                    checkList.Add(new Vector2(j, i), null);

                    continue;
                }

                Vector2 position;

                position.x = j * tileSize;
                position.y = i * tileSize;

                // if (j > 0)
                // {
                //     offset.x = Random.Range(0, 2) * 0.5f * tileSize;

                //     position += offset;
                // }

                BaseTile tile = Instantiate(tilePrefab, transform);

                tile.transform.position = position;

                int layer = Random.Range(0, 3);

                tile.SetLayer(layer);





                SpawnTileInfo spawnTileInfo = new SpawnTileInfo();

                spawnTileInfo.coordinator = new Vector2(j, i);
                spawnTileInfo.position = position;
                spawnTileInfo.layer = layer;

                checkList.Add(new Vector2(j, i), spawnTileInfo);

                _spawnedTiles.Add(tile);
            }
        }

        // Symetricalize
        for (int i = -row; i <= row; i++)
        {
            for (int j = -column; j <= column; j++)
            {
                if (i >= 0 && j >= 0)
                {
                    continue;
                }

                if (checkList[new Vector2(Mathf.Abs(j), Mathf.Abs(i))] != null)
                {
                    SpawnTileInfo spawnTileInfo = checkList[new Vector2(Mathf.Abs(j), Mathf.Abs(i))];

                    BaseTile tile = Instantiate(tilePrefab, transform);

                    Vector2 position = spawnTileInfo.position;

                    if (j < 0)
                    {
                        position.x *= -1;
                    }

                    if (i < 0)
                    {
                        position.y *= -1;
                    }

                    tile.transform.position = position;
                    tile.SetLayer(spawnTileInfo.layer);

                    _spawnedTiles.Add(tile);
                }
            }
        }

        // Faction
        List<int> poolFaction = new List<int>();

        for (int i = 0; i < _spawnedTiles.Count; i++)
        {
            int faction = Random.Range(0, 9);

            poolFaction.Add(faction);
        }

        for (int i = 0; i < _spawnedTiles.Count; i++)
        {
            _spawnedTiles[i].tileServiceLocator.tileProperty.Faction = poolFaction[i];
            _spawnedTiles[i].tileServiceLocator.tileUI.SetIcon(poolFaction[i]);
            // _spawnedTiles[i].tileServiceLocator.tileUI.SetFactionText(poolFaction[i]);
        }
    }

    private void GenerateLayer(int layer)
    {
        float tileSize = tilePrefab.GetComponent<TileUI>().TileSize;

        List<GameObject> spawnedTiles = HorizontalSymmetricGrid.GenerateGrid(tilePrefab.gameObject, 6, 7, tileSize, 0.65f);

        _spawnedTiles = new List<BaseTile>();

        for (int i = 0; i < spawnedTiles.Count; i++)
        {
            _spawnedTiles.Add(spawnedTiles[i].GetComponent<BaseTile>());
        }

        List<int> poolFaction = GetAllocatedFactions(_spawnedTiles.Count);

        for (int i = 0; i < _spawnedTiles.Count; i++)
        {
            _spawnedTiles[i].tileServiceLocator.tileProperty.Faction = poolFaction[i];
            _spawnedTiles[i].tileServiceLocator.tileUI.SetIcon(poolFaction[i]);
        }
    }

    public List<int> GetAllocatedFactions(int numTile)
    {
        List<int> factions = new List<int>();

        int counter = 0;
        int currentFaction = 0;

        for (int i = 0; i < numTile; i++)
        {
            if (counter % 3 == 0)
            {
                currentFaction = Random.Range(0, 9);
            }

            factions.Add(currentFaction);

            counter++;
        }

        return factions;
    }

    bool GetRandomPercentChance(int percent)
    {
        return Random.value <= percent / 100f;
    }
}

public static class HorizontalSymmetricGrid
{
    public static List<GameObject> GenerateGrid(GameObject prefab, int columns, int rows, float spacing, float fillRatio)
    {
        List<GameObject> spawnedObjects = new List<GameObject>();

        List<Vector2Int> leftHalf = new List<Vector2Int>();
        int midCol = columns / 2;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < midCol; x++)
                leftHalf.Add(new Vector2Int(x, y));

            if (columns % 2 != 0)
                leftHalf.Add(new Vector2Int(midCol, y));
        }

        ShuffleList(leftHalf);

        int maxPairs = columns % 2 == 0 ? leftHalf.Count : leftHalf.Count - rows;
        int maxCenter = columns % 2 == 0 ? 0 : rows;

        int maxFill = Mathf.FloorToInt((maxPairs * 2 + maxCenter) * fillRatio);

        int pairCount = 0;
        int centerCount = 0;

        if (columns % 2 == 0)
        {
            maxFill = RoundDownToDiv(maxFill, 3 * 2);

            pairCount = maxFill / 2;
            centerCount = 0;
        }
        else
        {
            maxFill = RoundDownToDiv(maxFill, 3);

            for (int c = Mathf.Min(maxCenter, maxFill); c >= 0; c--)
            {
                int remaining = maxFill - c;
                if (remaining < 0) continue;
                if (remaining % 2 != 0) continue;

                int p = remaining / 2;
                if (p <= maxPairs && (p * 2 + c) % 3 == 0)
                {
                    pairCount = p;
                    centerCount = c;
                    break;
                }
            }
        }

        int spawned = 0;

        for (int i = 0; i < pairCount; i++)
        {
            Vector2Int pos = leftHalf[i];
            Vector2Int mirror = new Vector2Int(columns - 1 - pos.x, pos.y);
            spawnedObjects.Add(SpawnAt(prefab, pos, columns, rows, spacing));
            spawnedObjects.Add(SpawnAt(prefab, mirror, columns, rows, spacing));
            spawned += 2;
        }

        if (centerCount > 0)
        {
            for (int i = 0; i < centerCount; i++)
            {
                Vector2Int centerPos = new Vector2Int(midCol, i);
                spawnedObjects.Add(SpawnAt(prefab, centerPos, columns, rows, spacing));
                spawned += 1;
            }
        }

        return spawnedObjects;
    }

    static GameObject SpawnAt(GameObject prefab, Vector2Int gridPos, int columns, int rows, float spacing)
    {
        float offsetX = (columns - 1) * spacing / 2f;
        float offsetY = (rows - 1) * spacing / 2f;
        Vector3 worldPos = new Vector3(gridPos.x * spacing - offsetX, gridPos.y * spacing - offsetY, 0);

        GameObject tile = Object.Instantiate(prefab, worldPos, Quaternion.identity);

        return tile;
    }

    static void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }

    static int RoundDownToDiv(int value, int divBy)
    {
        return value - (value % divBy);
    }
}

public class SpawnTileInfo
{
    public Vector2 coordinator;
    public Vector3 position;
    public int layer;
}
