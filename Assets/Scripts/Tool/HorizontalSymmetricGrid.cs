using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class HorizontalSymmetricGrid
{
    public static List<GameObject> GenerateGrid(GameObject prefab, Transform container, int columns, int rows, float spacing, float fillRatio)
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
            spawnedObjects.Add(SpawnAt(prefab, container, pos, columns, rows, spacing));
            spawnedObjects.Add(SpawnAt(prefab, container, mirror, columns, rows, spacing));
            spawned += 2;
        }

        if (centerCount > 0)
        {
            for (int i = 0; i < centerCount; i++)
            {
                Vector2Int centerPos = new Vector2Int(midCol, i);
                spawnedObjects.Add(SpawnAt(prefab, container, centerPos, columns, rows, spacing));
                spawned += 1;
            }
        }

        return spawnedObjects;
    }

    static GameObject SpawnAt(GameObject prefab, Transform container, Vector2Int gridPos, int columns, int rows, float spacing)
    {
        float offsetX = (columns - 1) * spacing / 2f;
        float offsetY = (rows - 1) * spacing / 2f;
        Vector3 worldPos = new Vector3(gridPos.x * spacing - offsetX, gridPos.y * spacing - offsetY, 0);

        GameObject tile = (GameObject)PrefabUtility.InstantiatePrefab(prefab, container);

        tile.transform.position = worldPos;

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
