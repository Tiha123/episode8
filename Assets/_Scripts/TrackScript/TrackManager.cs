using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using DG.Tweening;
using UnityEngine;

public class TrackManager : MonoBehaviour
{

    [SerializeField] Track TrackPrefab;
    [SerializeField] PlayerControl PlayerPrefab;

    [SerializeField] List<Track> TrackList = new List<Track>();

    [SerializeField] GameObject TrackStart, TrackFinish;

    [Range(0f, 50f)] public float scrollspeed = 10f;

    [SerializeField, Range(1, 100)] int TrackCount = 3;

    [Range(1, 5)] public int Countdown = 3;

    [SerializeField] float trackThreshold = 5f;

    [Range(0f, 10f)] public float TrackCurveParamX = 0f;
    [Range(0f, 10f)] public float TrackCurveParamY = 0f;

    [Range(0f, 0.5f)] public float CurveFrequencyX = 0.5f;
    [Range(0f, 10f)] public float CurveAmplitudeX = 0f;
    [Range(0f, 0.5f)] public float CurveFrequencyY = 0.5f;
    [Range(0f, 10f)] public float CurveAmplitudeY = 0f;

    int nameindex = 0;

    Transform camTransform;

    IngameUI gameUI;

    [HideField] public List<Transform> laneList;

    [SerializeField] Material TrackMaterial;


    int curveAmount;
    [HideField] public float elapsedTime = 0f;

    void Start()
    {
        gameUI = FindFirstObjectByType<IngameUI>(FindObjectsInactive.Include);
        // Object[] UIs=FindObjectsByType<IngameUI>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        // if (UIs!=null||UIs.Length>0)
        // {
        //     gameUI=UIs[0] as IngameUI;
        // }
        camTransform = Camera.main.transform;
        curveAmount = Shader.PropertyToID("_CurveAmount");

        SpawnInitialTrack();
        SpawnStartZone(3f);
        SpawnPlayer();

        StartCoroutine(CountdownTrack());
    }

    void Update()
    {
        if (GameManager.IsPlaying == false || GameManager.IsGameOver == true || GameManager.IsUIOpen == true)
        {
            return;
        }
        // ScrollTrack();
        GameManager.MoveDistance += scrollspeed * Time.smoothDeltaTime;
        SpawnFinishZone(18f);
        RepositionTrack();
        BendTrack();
    }

    void SpawnInitialTrack()
    {
        Vector3 pos = new Vector3(0f, 0f, camTransform.position.z);
        for (int i = 0; i < TrackCount; ++i)
        {
            Track temp = SpawnNextTrack(pos);
            pos = temp.ExitPoint.position;
        }
        BendTrack();
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
            Track lastTrack = TrackList[TrackList.Count - 1];
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
        laneList = temp.Lanes;
        return temp;
    }

    void BendTrack()
    {
        elapsedTime += Time.deltaTime;

        TrackCurveParamX = Mathf.Lerp(-CurveAmplitudeX, CurveAmplitudeX, Mathf.PerlinNoise1D(elapsedTime * CurveFrequencyX));

        TrackCurveParamY = Mathf.Lerp(-CurveAmplitudeY, CurveAmplitudeY, Mathf.PerlinNoise1D(TrackCurveParamX * CurveFrequencyY));

        TrackMaterial.SetVector(curveAmount, new Vector4(TrackCurveParamX, TrackCurveParamY, 0f, 0f));
    }

    public Track GetTrackByZ(float ZValue)
    {
        foreach (Track t in TrackList)
        {
            if (ZValue > t.EntryPoint.position.z && ZValue <= t.ExitPoint.position.z)
            {
                return t;
            }
        }

        return null;
    }
    IEnumerator CountdownTrack()
    {
        while (true)
        {
            yield return new WaitUntil(() => GameManager.IsUIOpen == false);

            gameUI.ShowInfo($"{Countdown--}", 1f);
            yield return new WaitForSeconds(1f);

            if (Countdown <= 0f)
            {
                gameUI.ShowInfo($"Game Start!", 2f);
                GameManager.IsPlaying = true;
                GameManager.IsGameOver = false;
                yield break;
            }

            yield return new WaitUntil(() => GameManager.IsUIOpen == false);
        }
    }

    void SpawnPlayer()
    {
        PlayerControl temp = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity, transform);
        temp.trackmgr = this;
    }

    public void SetPhase(PhaseSO phase)
    {
        DOVirtual.Float(scrollspeed, phase.speed, 1f, s => scrollspeed = s).SetEase(Ease.InOutSine);
    }

    void SpawnStartZone(float zpos = 3f)
    {
        Track T = GetTrackByZ(zpos);

        GameObject o = Instantiate(TrackStart, T.ObstacleRoot);

        Vector3 spawnPosition = new Vector3(0f, 0f, zpos);

        o.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
    }
    GameObject _finishZone;
    void SpawnFinishZone(float zpos = 18f)
    {
        if (_finishZone != null)
        {
            return;
        }
        if (GameManager.MoveDistance + zpos < GameManager.distanceFinish)
        {
            return;
        }

        Track T = GetTrackByZ(zpos);

        _finishZone = Instantiate(TrackFinish, T.ObstacleRoot);

        Vector3 spawnPosition = new Vector3(0f, 0f, zpos);

        _finishZone.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
    }
}
