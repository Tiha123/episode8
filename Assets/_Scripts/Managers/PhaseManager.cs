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

    [SerializeField] List<PhaseProfile> phaseProfiles=new List<PhaseProfile>();

    [SerializeField] PhaseProfile phaseNow;

    private IngameUI uiIngame;

    private TrackManager trackmrg;

    private ObstacleManager obstmrg;

    private CollectableManager collmrg;


    IEnumerator Start()
    {
        GetFinishLine();
        uiIngame=FindFirstObjectByType<IngameUI>(FindObjectsInactive.Include);
        trackmrg=FindFirstObjectByType<TrackManager>(FindObjectsInactive.Include);
        obstmrg=FindFirstObjectByType<ObstacleManager>(FindObjectsInactive.Include);
        collmrg=FindFirstObjectByType<CollectableManager>(FindObjectsInactive.Include);
        yield return new WaitUntil(()=>uiIngame!=null);
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
                SetPhase();
            }

        yield return new WaitForSeconds(updateInterval);

        }
    }

    void SetPhase()
    {

    }


    void GetFinishLine()
    {
        PhaseProfile phaseFinish=phaseProfiles.LastOrDefault();

        GameManager.distanceFinish=phaseFinish.Distance;
    }
}
