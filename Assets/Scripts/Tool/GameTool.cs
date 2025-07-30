using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class GameTool : EditorWindow
{
    private DefaultAsset targetFolder;
    private Sprite[] sprites;
    private Texture2D spriteSheet;

    #region SPECIFIC
    private GameObject tilePrefab;
    #endregion

    [MenuItem("Tools/Game Tool")]
    public static void ShowWindow()
    {
        GetWindow<GameTool>("Game Tool");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Auto Assign Sprites from Folder", EditorStyles.boldLabel);
        targetFolder = (DefaultAsset)EditorGUILayout.ObjectField("Target Folder", targetFolder, typeof(DefaultAsset), false);
        tilePrefab = (GameObject)EditorGUILayout.ObjectField("Prefab Tile", tilePrefab, typeof(GameObject), true);
        spriteSheet = (Texture2D)EditorGUILayout.ObjectField("Sprite Sheet", spriteSheet, typeof(Texture2D), false);

        if (GUILayout.Button("Assign Sprites"))
        {
            LoadAllSpritesFromSheet();
        }

        if (GUILayout.Button("Generate Level"))
        {
            GenerateLevel();
        }
    }

    #region AUTO ASSIGN
    // private void LoadAllSpritesFromFolder(string folderPath)
    // {
    //     string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { folderPath });
    //     List<Sprite> spriteList = new List<Sprite>();

    //     foreach (string guid in guids)
    //     {
    //         string assetPath = AssetDatabase.GUIDToAssetPath(guid);
    //         Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
    //         if (sprite != null)
    //         {
    //             spriteList.Add(sprite);
    //         }
    //     }

    //     sprites = spriteList.ToArray();

    //     prefabTileUI.spriteContainer = sprites;
    // }

    private void LoadAllSpritesFromSheet()
    {
        string path = "Assets/Sprites/Tile Icon/ChatGPT Image Jul 28, 2025, 10_06_53 PM.png";

        Object[] assets = AssetDatabase.LoadAllAssetRepresentationsAtPath(path);

        List<Sprite> sprites = new List<Sprite>();

        foreach (var asset in assets)
        {
            if (asset is Sprite sprite)
            {
                sprites.Add(sprite);
            }
        }

        tilePrefab.GetComponent<TileUI>().spriteContainer = sprites.ToArray();
    }
    #endregion

    #region LEVEL
    private void GenerateLevel()
    {
        Transform level = Selection.activeTransform;

        // Remove old content
        List<Transform> toRemoveList = new List<Transform>();

        for (int i = 0; i < level.childCount; i++)
        {
            toRemoveList.Add(level.GetChild(i));
        }

        foreach (var item in toRemoveList)
        {
            DestroyImmediate(item.gameObject, true);
        }
        //

        for (int i = 0; i <= 2; i++)
        {
            GenerateLayer(i, level);
        }

        EditorUtility.SetDirty(level);
    }

    private void GenerateLayer(int layer, Transform container)
    {
        float tileSize = tilePrefab.GetComponent<TileUI>().TileSize;

        List<GameObject> spawnedTiles = HorizontalSymmetricGrid.GenerateGrid(tilePrefab.gameObject, container, 6 + layer % 2, 7 + layer % 2, tileSize, 0.5f);

        List<BaseTile> _spawnedTileComponents = new List<BaseTile>();

        for (int i = 0; i < spawnedTiles.Count; i++)
        {
            _spawnedTileComponents.Add(spawnedTiles[i].GetComponent<BaseTile>());

            Vector3 position = spawnedTiles[i].transform.position;

            position.z = layer;

            spawnedTiles[i].transform.position = position;
        }

        List<int> poolFaction = RandomizeFactionForTiles(_spawnedTileComponents.Count);

        for (int i = 0; i < _spawnedTileComponents.Count; i++)
        {
            _spawnedTileComponents[i].tileServiceLocator.tileProperty.Faction = poolFaction[i];
            _spawnedTileComponents[i].tileServiceLocator.tileUI.SetIcon(poolFaction[i]);
        }
    }

    public List<int> RandomizeFactionForTiles(int numTile)
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
    #endregion
}
