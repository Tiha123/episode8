using Unity.Mathematics;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    [SerializeField] float horzspeed;

    int currentLane;
    [HideInInspector] public TrackManager trackmgr;
    bool horz;
    Vector3 pos;

    void Update()
    {
        currentLane = math.clamp(currentLane, 0, trackmgr.laneList.Count - 1);
        currentLane = trackmgr.laneList.Count / 2;
        if (Input.GetKeyDown(KeyCode.D))
        {
            currentLane += 1;
            pos=new Vector3(trackmgr.laneList[currentLane].transform.position.x,transform.position.y,transform.position.z);
            transform.position=pos;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            currentLane -= 1;
            pos=new Vector3(trackmgr.laneList[currentLane].transform.position.x,transform.position.y,transform.position.z);
            transform.position=pos;
        }
        else
        {

        }
        Debug.Log(currentLane);
        //transform.position += Vector3.right*horz*Time.deltaTime*horzspeed;
    }
}
