using PrimeTween;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Select();
        }
    }

    private void Select()
    {
        Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            BaseTile tile = hit.collider.GetComponent<BaseTile>();

            if (tile != null)
            {
                SpaceManager.Instance.Fill(tile);
            }
        }
    }
}
