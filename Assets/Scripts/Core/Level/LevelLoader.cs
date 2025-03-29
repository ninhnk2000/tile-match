using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private BaseTile tilePrefab;

    private List<BaseTile> _spawnedTiles;

    private void Awake()
    {
        GenerateBoard();
    }

    private void GenerateBoard()
    {
        int row = 3;
        int column = 3;

        float tileSize = tilePrefab.GetComponent<SpriteRenderer>().bounds.size.x;

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

                if (j > 0)
                {
                    offset.x = Random.Range(0, 2) * 0.5f * tileSize;

                    position += offset;
                }

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

        List<int> poolFaction = new List<int>();

        for (int i = 0; i < _spawnedTiles.Count / 2; i++)
        {
            int faction = Random.Range(0, 9);

            poolFaction.Add(faction);
            poolFaction.Add(faction);
        }

        for (int i = 0; i < _spawnedTiles.Count; i++)
        {
            _spawnedTiles[i].tileServiceLocator.tileUI.SetFactionText(poolFaction[i]);
        }
    }

    bool GetRandomPercentChance(int percent)
    {
        return Random.value <= percent / 100f;
    }
}

public class SpawnTileInfo
{
    public Vector2 coordinator;
    public Vector3 position;
    public int layer;
}
