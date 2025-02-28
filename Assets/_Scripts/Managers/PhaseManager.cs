using UnityEngine;
using CustomInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhaseManager : MonoBehaviour
{
    [HorizontalLine("기본속성"), HideField] public bool _l0;
    [SerializeField] float updateInterval=1f;
    [HorizontalLine("트랙속성"), HideField] public bool _l1;
    [SerializeField,Foldout] List<PhaseSO> phaseProfiles=new List<PhaseSO>();

    public List<PhaseSO> distances=> phaseProfiles; // 읽기전용 외부변수

    PhaseSO phaseNow;

    private IngameUI uiIngame;

    private TrackManager trackmrg;

    private ObstacleManager obstmrg;

    private CollectableManager collmrg;


    IEnumerator Start()
    {
        uiIngame=FindFirstObjectByType<IngameUI>(FindObjectsInactive.Include);
        trackmrg=FindFirstObjectByType<TrackManager>(FindObjectsInactive.Include);
        obstmrg=FindFirstObjectByType<ObstacleManager>(FindObjectsInactive.Include);
        collmrg=FindFirstObjectByType<CollectableManager>(FindObjectsInactive.Include);
        GetFinishLine();
        uiIngame.setDistance(distances);
        yield return new WaitUntil(()=>GameManager.IsGameOver);
        StartCoroutine(IntervalUpdate());
    }


    IEnumerator IntervalUpdate()
    {
        if (phaseProfiles==null||phaseProfiles.Count<=0)
        {
            yield break;
        }

        int i=0;

        while (true)
        {
            phaseNow=phaseProfiles[i];
            if (GameManager.MoveDistance>phaseNow.Distance)
            {
                i++;
                SetPhase(phaseNow);
            }

            if (i >= distances.Count)
            {
                GameClear(phaseNow);
                yield break;
            }

        yield return new WaitForSeconds(updateInterval);

        }
    }

    void GetFinishLine()
    {
        PhaseSO phaseFinish=phaseProfiles.LastOrDefault();

        GameManager.distanceFinish=phaseFinish.Distance;
    }
    void SetPhase(PhaseSO phaseNow)
    {
        uiIngame?.SetPhase(phaseNow);
        obstmrg?.SetPhase(phaseNow);
        collmrg?.SetPhase(phaseNow);
        trackmrg?.SetPhase(phaseNow);
    }

    void GameClear(PhaseSO phase)
    {
        SetPhase(phase);

        GameManager.IsPlaying = false;
        GameManager.IsGameOver = true;
    }

}
