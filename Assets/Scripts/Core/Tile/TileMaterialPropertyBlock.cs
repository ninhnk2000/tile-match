using UnityEngine;

public class TileMaterialPropertyBlock : MonoBehaviour
{
    #region PRIVATE FIELD
    [SerializeField] private Renderer _renderer;
    private MaterialPropertyBlock _propertyBlock;
    #endregion

    private void Awake()
    {
        Init();

        SetOutlineWidth(0);
    }

    private void Init()
    {
        if (_renderer == null)
        {
            _renderer = GetComponent<Renderer>();
        }

        if (_propertyBlock == null)
        {
            _propertyBlock = new MaterialPropertyBlock();
        }
    }

    public void SetOutlineWidth(float value)
    {
        _propertyBlock.SetFloat("_Thickness", value);

        _renderer.SetPropertyBlock(_propertyBlock);
    }
}