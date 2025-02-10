using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{

    [SerializeField] Track TrackPrefab;

    private List<Track> TrackList = new List<Track>();

    [SerializeField, Range(0f, 50f)] float scrollspeed = 10f;

    [SerializeField, Range(1, 10)] int TrackCount = 3;

    [SerializeField] float trackThreshold=0f;

    int nameindex=0;

    Transform camTransform;

    void Start()
    {
        camTransform=Camera.main.transform;
        SpawnInitialTrack();
    }

    void Update()
    {
        ScrollTrack();
        RepositionTrack();
    }

    void SpawnInitialTrack()
    {
        for (int i = 0; i < TrackCount; ++i)
        {
            Vector3 pos = i == 0 ? Vector3.zero : TrackList[i - 1].ExitPoint.position;
            Track temp = Instantiate(TrackPrefab, pos, Quaternion.identity, transform);
            TrackList.Add(temp);
            TrackList[i].name = $"Track_{i}";
            nameindex++;
        }
    }

    void ScrollTrack()
    {
        foreach (Track i in TrackList)
        {
            if (i != null)
            {
                i.transform.position += Vector3.back * Time.deltaTime * scrollspeed;
            }
        }

    }

    void RepositionTrack()
    {
        if (TrackList.Count > 0)
        {
            if (TrackList[0].ExitPoint.position.z < camTransform.position.z-trackThreshold)
            {

                Destroy(TrackList[0].gameObject);
                TrackList.RemoveAt(0);
                SpawnNextTrack(TrackList[TrackList.Count-1]);
            }
        }
    }

    void SpawnNextTrack(Track current)
    {
        Track temp;
        temp = Instantiate(TrackPrefab, current.ExitPoint.position, Quaternion.identity, transform);
        TrackList.Add(temp);
        temp.name = $"Track_{nameindex}";
        nameindex++;
    }
}
