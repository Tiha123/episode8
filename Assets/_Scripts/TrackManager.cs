using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{

    [SerializeField] Track TrackPrefab;

    private List<Track> TrackList = new List<Track>();

    [Range(0f, 50f)] public float scrollspeed = 10f;

    [SerializeField, Range(1, 100)] int TrackCount = 3;

    [SerializeField] float trackThreshold = 0f;

    int nameindex = 0;

    Transform camTransform;

    void Start()
    {
        camTransform = Camera.main.transform;
        SpawnInitialTrack();
    }

    void Update()
    {
        // ScrollTrack();
        RepositionTrack();
    }

    void SpawnInitialTrack()
    {
        Vector3 pos = new Vector3(0f,0f,camTransform.position.z);
        for (int i = 0; i < TrackCount; ++i)
        {
            Track temp = SpawnNextTrack(pos);
            pos = temp.ExitPoint.position;
        }
    }

    // void ScrollTrack()
    // {
    //     if (TrackList[0] != null)
    //     {
    //         foreach (Track i in TrackList)
    //         {

    //             i. transform.position += Vector3.back * Time.deltaTime * scrollspeed;

    //         }
    //     }

    // }

    void RepositionTrack()
    {
        if (TrackList.Count <= 0)
        {
            return;
        }
        if (TrackList[0].EntryPoint.position.z < camTransform.position.z - trackThreshold)
        {
            Destroy(TrackList[0].gameObject);
            TrackList.RemoveAt(0);
            Track lastTrack=TrackList[TrackList.Count - 1];
            SpawnNextTrack(lastTrack.ExitPoint.position);
        }
    }

    Track SpawnNextTrack(Vector3 pos)
    {
        Track temp;
        temp = Instantiate(TrackPrefab, pos, Quaternion.identity, transform);
        temp.trackmgr = this;
        TrackList.Add(temp);
        temp.name = $"Track_{nameindex}";
        return temp;
    }
}
