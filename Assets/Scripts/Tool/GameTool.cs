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
    private GameObject prefabTile;
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
        prefabTile = (GameObject)EditorGUILayout.ObjectField("Prefab Tile", prefabTile, typeof(GameObject), true);
        spriteSheet = (Texture2D)EditorGUILayout.ObjectField("Sprite Sheet", spriteSheet, typeof(Texture2D), false);

        if (GUILayout.Button("Assign Sprites"))
        {
            LoadAllSpritesFromSheet();
        }
    }

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
                Debug.Log("SAFERIO " + sprite);
                sprites.Add(sprite);
            }
        }

        Debug.Log("SAFERIO " + sprites.Count);

        prefabTile.GetComponent<TileUI>().spriteContainer = sprites.ToArray();
    }
}
