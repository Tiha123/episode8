using UnityEngine;
using System.Collections;

public class CollectableCoin : Collectable
{
    public uint addVal = 1;
    [SerializeField] Transform pivot;
    [SerializeField] ParticleSystem particle;
    public override void SetLandPosition(int laneNum, float zpos, TrackManager trackmgr)
    {
        laneNum = Mathf.Clamp(laneNum, 0, trackmgr.laneList.Count - 1);
        Transform laneTransform = trackmgr.laneList[laneNum];
        Vector3 spawnPosition = new Vector3(laneTransform.position.x, laneTransform.position.y, zpos);
        transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
    }
    public override void Collect()
    {
        
        GameManager.Coin += addVal;
        StartCoroutine(Disappear());
        
    }

    IEnumerator Disappear()
    {
        this.transform.SetParent(null);
        pivot.gameObject.SetActive(false);
        particle.Play();
        yield return new WaitUntil(()=>particle.isPlaying==false);
        Destroy(this.gameObject);
    }
}
