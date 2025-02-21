using UnityEngine;
using DG.Tweening;

public class CollectableCoin : Collectable
{
    public uint addVal = 1;
    public Transform pivot;
    public override void SetLandPosition(int laneNum, float zpos, TrackManager trackmgr)
    {
        laneNum = Mathf.Clamp(laneNum, 0, trackmgr.laneList.Count - 1);
        Transform laneTransform = trackmgr.laneList[laneNum];
        Vector3 spawnPosition = new Vector3(laneTransform.position.x, laneTransform.position.y, zpos);
        transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
    }
    public override void Collect()
    {
        this.gameObject.transform.SetParent(null);
        GameManager.Coin += addVal;
        Sequence _seq=DOTween.Sequence();
        _seq.Append(pivot.transform.DOMoveY(0.5f, 0.5f));
        _seq.Join(pivot.transform.DORotate(new Vector3(0f, 360f, 0f), 0.5f, RotateMode.FastBeyond360));
        _seq.Join(pivot.transform.DOScale(1.1f, 0.5f));
        _seq.AppendCallback(()=>Destroy(gameObject));
    }
}
