using Deform;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    [SerializeField] float moveDuration = 0.5f;
    // [SerializeField] AnimationCurve jumpCurve;
    [SerializeField] float jumpHeight = 1.5f;
    [SerializeField] float jumpDuration = 0.5f;
    [SerializeField] float slideDuration=0.5f;
    [SerializeField] Ease moveEase;
    [SerializeField] Ease jumpEase;
    [SerializeField] Transform pivot;
    [Space(20), SerializeField] SquashAndStretchDeformer slideLeftDeform;
    [SerializeField] SquashAndStretchDeformer slideRightDeform;
    [SerializeField] SquashAndStretchDeformer jumpUpDeform;
    [SerializeField] SquashAndStretchDeformer jumpDownDeform;
    [SerializeField] SquashAndStretchDeformer slideDeform;
    bool isVertical = false;
    bool isMoving = false;

    // float jumpStartTime;
    // float elapsedTime;
    // float p;

    int currentLane;

    private Sequence seqMove;
    private Sequence seqJump;

    [HideInInspector] public TrackManager trackmgr;
    Vector3 pos;

    void Start()
    {
        currentLane = trackmgr.laneList.Count / 2;
    }

    // void Update()
    // {
    //     if (Input.GetButtonDown("Right") && isVertical == false)
    //     {
    //         HandlePlayer(1);
    //     }
    //     else if (Input.GetButtonDown("Left") && isVertical == false)
    //     {
    //         HandlePlayer(-1);
    //     }

    //     if (Input.GetButtonDown("Jump") && isVertical == false)
    //     {
    //         jumpStartTime = Time.time;
    //         isVertical = true;
    //     }
    //     UpdateJump();
    //     UpdatePosition();
    //     //transform.position += Vector3.right*horz*Time.deltaTime*horzspeed;
    // }

    void Update()
    {
        if (pivot == null)
        {
            return;
        }
        if (Input.GetButtonDown("Jump"))
        {
            HandleJump();
        }
        else if (Input.GetButtonDown("Slide"))
        {
            HandleSlide();
        }
        else if (Input.GetButtonDown("Left"))
        {
            HandlePlayer(-1);
        }
        else if (Input.GetButtonDown("Right"))
        {
            HandlePlayer(1);
        }
        

    }


    void HandlePlayer(int direction)
    {
        if (isVertical==true) return;
        currentLane += direction;
        if (currentLane < 0 || currentLane > trackmgr.laneList.Count - 1)
        {
            currentLane = math.clamp(currentLane, 0, trackmgr.laneList.Count - 1);
            pos = new Vector3(trackmgr.laneList[currentLane].transform.position.x, pivot.position.y, pivot.position.z);
        }
        else
        {
            isMoving = true;

            var squash = direction switch
            {
                -1 => slideLeftDeform,
                1 => slideRightDeform,
                _ => null
            };

            if (seqMove != null)
            {
                seqMove.Kill(true);
            }

            currentLane = math.clamp(currentLane, 0, trackmgr.laneList.Count - 1);
            pos = new Vector3(trackmgr.laneList[currentLane].transform.position.x, pivot.position.y, pivot.position.z);
            seqMove = DOTween.Sequence().OnComplete(() => squash.Factor = 0);
            seqMove.Append(pivot.DOMove(pos, moveDuration).SetEase(moveEase).OnComplete(() => isMoving = false));
            seqMove.Join(DOVirtual.Float(0f, 1f, moveDuration / 2, (v) => squash.Factor = v));
            seqMove.Insert(moveDuration / 2, DOVirtual.Float(1f, 0f, moveDuration / 2, (v) => squash.Factor = v));
        }





    }

    void HandleJump()
    {
        if (isVertical==true||isMoving==true) return;
        isVertical = true;
        if (seqJump != null)
            {
                seqJump.Kill(true);
            }
        seqJump = DOTween.Sequence().OnComplete(() => jumpUpDeform.Factor = 0).OnComplete(() => jumpDownDeform.Factor = 0);
        seqJump.Append(pivot.DOLocalJump(pos, jumpHeight, 1, jumpDuration).SetEase(jumpEase).OnComplete(() => isVertical = false));
        seqJump.Join(DOVirtual.Float(0f, 1f, jumpDuration / 4, (v) => jumpUpDeform.Factor = v));
        seqJump.Insert(jumpDuration / 4, DOVirtual.Float(1f, 0f, jumpDuration / 4, (v) => jumpUpDeform.Factor = v));
        seqJump.Insert(jumpDuration / 2, DOVirtual.Float(0f, 1f, jumpDuration / 4, (v) => jumpDownDeform.Factor = v));
        seqJump.Insert(jumpDuration * 3 / 4, DOVirtual.Float(1f, 0f, jumpDuration / 4, (v) => jumpDownDeform.Factor = v));
    }

    void HandleSlide()
    {
        if(isVertical==true||isMoving==true) return;
        isVertical = true;
        if (seqJump != null)
            {
                seqJump.Kill(true);
            }
        seqJump = DOTween.Sequence().OnComplete(() => jumpUpDeform.Factor = 0).OnComplete(() => jumpDownDeform.Factor = 0);
        seqJump.Append(DOVirtual.Float(0f, -1f, slideDuration / 2, (v) => slideDeform.Factor = v));
        seqJump.Append(DOVirtual.Float(-1f, 0f, slideDuration / 2, (v) => slideDeform.Factor = v).OnComplete(() => isVertical = false));

    }

    // void UpdatePosition()
    // {
    //     transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime);
    // }

    // void UpdateJump()
    // {
    //     if (isVertical == true)
    //     {
    //         elapsedTime = Time.time - jumpStartTime;
    //         p = Mathf.Clamp(elapsedTime / jumpDuration, 0f, 1f);
    //         float heightnow = jumpHeight * jumpCurve.Evaluate(p);
    //         pos.y = heightnow;
    //     }

    //     if (p >= 1f)
    //     {
    //         isVertical = false;
    //         pos.y = 0f;
    //     }
    // }
}
