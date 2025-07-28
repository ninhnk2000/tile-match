using PrimeTween;
using UnityEngine;
using UnityEngine.TextCore;

public class BasicSpace : BaseSpace
{
    public override void Interact(BaseTile baseTile)
    {
        base.Interact(baseTile);

        baseTile.transform.SetParent(transform);

        Tween.LocalPosition(baseTile.transform, new Vector3(0, 0, -1), duration: 0.3f)
        .OnComplete(() =>
        {
            SpaceManager.Instance.CheckClearTiles();
        });
        Tween.Scale(baseTile.transform, 0.3f, duration: 0.3f);

        IsFilled = true;
    }
}
