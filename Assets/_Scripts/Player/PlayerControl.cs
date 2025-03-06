using System.Collections.Generic;
using Deform;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;
public enum PlayerMove { Idle = 0, Move = 1, Jump = 2, Slide = 3 };
public enum PlayerState { INVINCIBLE = 1 << 0, MAGNETIC = 1 << 1};

public class PlayerControl : MonoBehaviour
{

    [SerializeField] float moveDuration = 0.5f;
    // [SerializeField] AnimationCurve jumpCurve;
    [SerializeField] float jumpHeight = 1.5f;
    [SerializeField] float jumpDuration = 0.5f;
    [SerializeField] float slideDuration = 0.5f;
    [SerializeField] Ease moveEase;
    [SerializeField] Ease jumpEase;
    [SerializeField] Transform pivot;
    [Space(20), SerializeField] SquashAndStretchDeformer slideLeftDeform;
    [SerializeField] SquashAndStretchDeformer slideRightDeform;
    [SerializeField] SquashAndStretchDeformer jumpUpDeform;
    [SerializeField] SquashAndStretchDeformer jumpDownDeform;
    [SerializeField] SquashAndStretchDeformer slideDeform;
    [SerializeField] List<Collider> CollidersList; //0>기본, 1>슬라이드

    // float jumpStartTime;
    // float elapsedTime;
    // float p;

    int currentLane;

    private Sequence seqMove;
    private Sequence seqJump;

    [HideInInspector] public TrackManager trackmgr;
    Vector3 pos;


    public PlayerMove move = PlayerMove.Idle;

    int curveAmount;
    [Space(20)]
    [SerializeField] MMF_Player feedbackImpact;
    [SerializeField] MMF_Player feedbackCrash;
    [SerializeField] MMF_Player feedbackInvincible;

    void Start()
    {
        currentLane = trackmgr.laneList.Count / 2;
        curveAmount = Shader.PropertyToID("_CurveAmount");
        CollidersList.ForEach(p =>
        {
            p.gameObject.SetActive(false);
        });
        SwitchState(move, out move, PlayerMove.Idle);;
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
        #if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.life=3;
        }
        #endif
        if (pivot == null || GameManager.IsPlaying == false || GameManager.IsGameOver == true)
        {
            return;
        }
        if (Input.GetButton("Jump"))
        {
            HandleJump();
        }
        else if (Input.GetButton("Slide"))
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
        if (move == PlayerMove.Jump || move == PlayerMove.Slide) return;
        currentLane += direction;
        if (currentLane < 0 || currentLane > trackmgr.laneList.Count - 1)
        {
            currentLane = Mathf.Clamp(currentLane, 0, trackmgr.laneList.Count - 1);
            pos = new Vector3(trackmgr.laneList[currentLane].transform.position.x, pivot.position.y, pivot.position.z);
        }
        else
        {
            SwitchState(move, out move, PlayerMove.Move);

            var squash = direction switch
            {
                -1 => slideLeftDeform,
                1 => slideRightDeform,
                _ => null
            };

            if (seqMove != null)
            {
                seqMove.Kill(true);
                SwitchState(move, out move, PlayerMove.Move);
            }

            currentLane = Mathf.Clamp(currentLane, 0, trackmgr.laneList.Count - 1);
            pos = new Vector3(trackmgr.laneList[currentLane].transform.position.x, pivot.position.y, pivot.position.z);
            pivot.DOMove(pos, moveDuration).SetEase(moveEase).OnComplete(() => SwitchState(move, out move, PlayerMove.Idle));

            seqMove = DOTween.Sequence().OnComplete(() => squash.Factor = 0);
            seqMove.Append(DOVirtual.Float(0f, 1f, moveDuration / 2, (v) => squash.Factor = v));
            seqMove.Append(DOVirtual.Float(1f, 0f, moveDuration / 2, (v) => squash.Factor = v));
        }
    }

    void HandleJump()
    {
        if (move != PlayerMove.Idle) return;

        SwitchState(move, out move, PlayerMove.Jump);

        pivot.DOLocalJump(pos, jumpHeight, 1, jumpDuration).SetEase(jumpEase).OnComplete(()=>SwitchState(move, out move, PlayerMove.Idle));


        seqJump = DOTween.Sequence().OnComplete(() => jumpUpDeform.Factor = 0).OnComplete(() => jumpDownDeform.Factor = 0);
        seqJump.Append(DOVirtual.Float(0f, 1f, jumpDuration / 4, (v) => jumpUpDeform.Factor = v));
        seqJump.Append(DOVirtual.Float(1f, 0f, jumpDuration / 4, (v) => jumpUpDeform.Factor = v));
        seqJump.Append(DOVirtual.Float(0f, 1f, jumpDuration / 4, (v) => jumpDownDeform.Factor = v));
        seqJump.Append(DOVirtual.Float(1f, 0f, jumpDuration / 4, (v) => jumpDownDeform.Factor = v));
    }

    void HandleSlide()
    {
        if (move != PlayerMove.Idle) return;
        SwitchState(move, out move, PlayerMove.Slide);
        seqJump = DOTween.Sequence().OnComplete(() => jumpUpDeform.Factor = 0).OnComplete(() => jumpDownDeform.Factor = 0);
        seqJump.Append(DOVirtual.Float(0f, -1f, slideDuration / 2, (v) => slideDeform.Factor = v));
        seqJump.Append(DOVirtual.Float(-1f, 0f, slideDuration / 2, (v) => slideDeform.Factor = v).OnComplete(()=>SwitchState(move, out move, PlayerMove.Idle)));
    }

    void SwitchState(PlayerMove pState, out PlayerMove outState, PlayerMove changeState)
    {
        CollidersList[(int)pState].gameObject.SetActive(false);
        CollidersList[(int)changeState].gameObject.SetActive(true);
        outState = changeState;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other)
        {
            if (other.tag == "Collectable")
            {
                feedbackImpact?.PlayFeedbacks();
                var c = other.GetComponentInParent<Collectable>();
                other.enabled = false;
                c?.Collect();

            }
            else if (other.tag == "Obstacle"&&!GameManager.playerstate.HasAny(PlayerState.INVINCIBLE))
            {
                feedbackCrash?.PlayFeedbacks();
                feedbackInvincible?.PlayFeedbacks();
            }
            other.enabled = false;
        }
    }

    public void OnInvincible(bool b)
    {
        if(b)
        {
            GameManager.playerstate|=PlayerState.INVINCIBLE;//추가
        }
        else
        {
            GameManager.playerstate&= ~PlayerState.INVINCIBLE;//제거
        }
    }
    public void OnCrash(bool b)
    {
        
        if (b)
        {   
            GameManager.life--;
        }
        GameManager.IsPlaying= !b;

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
