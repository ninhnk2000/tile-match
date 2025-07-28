using PrimeTween;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private Camera _mainCamera;

    private BaseTile[] _selectingTiles;

    private void Awake()
    {
        _mainCamera = Camera.main;

        _selectingTiles = new BaseTile[2];
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Select();
        }

        // if (Input.GetMouseButtonUp(0))
        // {
        //     if (_selectingTile != null)
        //     {
        //         _selectingTile.tileServiceLocator.tileMaterialPropertyBlock.SetOutlineWidth(0);

        //         _selectingTile = null;
        //     }
        // }
    }

    private void Select()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            BaseTile tile = hit.collider.GetComponent<BaseTile>();

            if (tile != null)
            {
                if (_selectingTiles[0] == null)
                {
                    _selectingTiles[0] = tile;

                    tile.tileServiceLocator.tileMaterialPropertyBlock.SetOutlineWidth(10f);
                }
                else if (_selectingTiles[1] == null && tile != _selectingTiles[0])
                {
                    // Debug.Log("SAFERIO " + tile.faction + "/" + tile.layer);
                    // Debug.Log("SAFERIO " + _selectingTiles[0].faction + "/" + _selectingTiles[0].layer);

                    TileProperty tileProperty = tile.tileServiceLocator.tileProperty;
                    TileProperty selectedTileProperty = _selectingTiles[0].tileServiceLocator.tileProperty;

                    if (tileProperty.Faction == selectedTileProperty.Faction && tileProperty.Layer == selectedTileProperty.Layer)
                    {
                        _selectingTiles[1] = tile;

                        // tile.tileServiceLocator.tileMaterialPropertyBlock.SetOutlineWidth(10f);

                        Tween.LocalPosition(tile.transform, _selectingTiles[0].transform.position, duration: 0.3f)
                            .OnComplete(() =>
                            {
                                tile.gameObject.SetActive(false);

                                _selectingTiles[0].tileServiceLocator.tileMaterialPropertyBlock.SetOutlineWidth(0);

                                _selectingTiles[0] = null;
                                _selectingTiles[1] = null;
                            });
                    }
                    else
                    {
                        Tween.ShakeScale(tile.transform, 0.3f * Vector3.one, duration: 0.3f);
                    }
                }
            }

            // if (_selectingTile != tile && _selectingTile != null)
            // {
            //     _selectingTile.tileServiceLocator.tileMaterialPropertyBlock.SetOutlineWidth(0);
            // }

            // _selectingTile = tile;
        }
        else
        {
            if (_selectingTiles[0] != null)
            {
                _selectingTiles[0].tileServiceLocator.tileMaterialPropertyBlock.SetOutlineWidth(0);

                _selectingTiles[0] = null;
            }
        }
    }
}
