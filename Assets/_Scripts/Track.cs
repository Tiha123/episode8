using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    public Transform EntryPoint;
    public Transform ExitPoint;
    public List<Transform> Lanes=new List<Transform>();

    [HideInInspector] public TrackManager trackmgr;

    void LateUpdate()
    {
        scroll();
    }

    void scroll()
    {
        if (trackmgr != null)
        {
            transform.position += Vector3.back * Time.smoothDeltaTime * trackmgr.scrollspeed;
        }
        // Time.deltaTime 매 프레임당 1번 호출될때의 간격
        // Time.fixedDeltaTime FixedUpdate(물리연산)기준, 0.02 간격(기본)
        // Time.smoothDeltaTime deltaTime 평균, 값이 고르게 나옴
        // fixed < delta < smooth
    }
}
