using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    [SerializeField] float moveDuration = 0.5f;
    // [SerializeField] AnimationCurve jumpCurve;
    [SerializeField] float jumpHeight = 1.5f;
    [SerializeField] float jumpDuration = 0.5f;
    [SerializeField] Ease moveEase;
    [SerializeField] Ease jumpEase;

    bool isJump = false;
    bool isMoving = false;

    // float jumpStartTime;
    // float elapsedTime;
    // float p;

    int currentLane;
    [HideInInspector] public TrackManager trackmgr;
    Vector3 pos;

    void Start()
    {
        currentLane = trackmgr.laneList.Count / 2;
    }

    // void Update()
    // {
    //     if (Input.GetButtonDown("Right") && isJump == false)
    //     {
    //         HandlePlayer(1);
    //     }
    //     else if (Input.GetButtonDown("Left") && isJump == false)
    //     {
    //         HandlePlayer(-1);
    //     }

    //     if (Input.GetButtonDown("Jump") && isJump == false)
    //     {
    //         jumpStartTime = Time.time;
    //         isJump = true;
    //     }
    //     UpdateJump();
    //     UpdatePosition();
    //     //transform.position += Vector3.right*horz*Time.deltaTime*horzspeed;
    // }

    void Update()
    {
        if (Input.GetButton("Jump")&&isMoving==false&&isJump==false)
        {
            isJump = true;
            HandleJump();
        }
        else if (Input.GetButton("Left")&&isMoving==false&&isJump==false)
        {
            isMoving = true;
            HandlePlayer(-1);
        }
        else if (Input.GetButton("Right")&&isMoving==false&&isJump==false)
        {
            isMoving = true;
            HandlePlayer(1);
        }

    }

    void HandlePlayer(int direction)
    {
        currentLane += direction;
        currentLane = math.clamp(currentLane, 0, trackmgr.laneList.Count - 1);
        pos = new Vector3(trackmgr.laneList[currentLane].transform.position.x, transform.position.y, transform.position.z);

        transform.DOMove(pos, moveDuration)
            .SetEase(moveEase)
            .OnComplete(() =>
            {
                isMoving = false;
                isJump = false;
            }
                );

    }

    void HandleJump()
    {
        transform.DOLocalJump(pos, jumpHeight, 1, jumpDuration)
                .SetEase(jumpEase)
                .OnComplete(() =>
                {
                    isMoving = false;
                    isJump = false;
                }
                            );
    }

    // void UpdatePosition()
    // {
    //     transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime);
    // }

    // void UpdateJump()
    // {
    //     if (isJump == true)
    //     {
    //         elapsedTime = Time.time - jumpStartTime;
    //         p = Mathf.Clamp(elapsedTime / jumpDuration, 0f, 1f);
    //         float heightnow = jumpHeight * jumpCurve.Evaluate(p);
    //         pos.y = heightnow;
    //     }

    //     if (p >= 1f)
    //     {
    //         isJump = false;
    //         pos.y = 0f;
    //     }
    // }
}
