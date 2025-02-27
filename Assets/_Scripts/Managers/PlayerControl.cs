using System.Collections.Generic;
using Deform;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;

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

    public enum PlayerState { Idle = 0, Move = 1, Jump = 2, Slide = 3 };

    public PlayerState state = PlayerState.Idle;

    [SerializeField] Material CarMaterial;
    [SerializeField] Material CollectMaterial;
    int curveAmount;
    [Space(20)]
    [SerializeField] MMF_Player feedbackImpact;
    [SerializeField] MMF_Player feedbackCrash;

    void Start()
    {
        currentLane = trackmgr.laneList.Count / 2;
        curveAmount = Shader.PropertyToID("_CurveAmount");
        CollidersList.ForEach(p=>
        {
            p.gameObject.SetActive(false);
        });
        SwitchState(state, out state, PlayerState.Idle);
        BendThings(CarMaterial);
        BendThings(CollectMaterial);
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.IsPlaying = !GameManager.IsPlaying;
            GameManager.IsGameOver = !GameManager.IsGameOver;
            GameManager.life=3;
        }
        if (GameManager.IsPlaying == false)
        {
            return;
        }
        if (pivot == null)
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
        BendThings(CarMaterial);
        BendThings(CollectMaterial);
    }


    void HandlePlayer(int direction)
    {
        if (state == PlayerState.Jump || state == PlayerState.Slide) return;
        currentLane += direction;
        if (currentLane < 0 || currentLane > trackmgr.laneList.Count - 1)
        {
            currentLane = Mathf.Clamp(currentLane, 0, trackmgr.laneList.Count - 1);
            pos = new Vector3(trackmgr.laneList[currentLane].transform.position.x, pivot.position.y, pivot.position.z);
        }
        else
        {
            SwitchState(state, out state, PlayerState.Move);

            var squash = direction switch
            {
                -1 => slideLeftDeform,
                1 => slideRightDeform,
                _ => null
            };

            if (seqMove != null)
            {
                seqMove.Kill(true);
                SwitchState(state, out state, PlayerState.Move);
            }

            currentLane = Mathf.Clamp(currentLane, 0, trackmgr.laneList.Count - 1);
            pos = new Vector3(trackmgr.laneList[currentLane].transform.position.x, pivot.position.y, pivot.position.z);
            seqMove = DOTween.Sequence().OnComplete(() => squash.Factor = 0);
            seqMove.Append(pivot.DOMove(pos, moveDuration).SetEase(moveEase).OnComplete(() => SwitchState(state, out state, PlayerState.Idle)));
            seqMove.Join(DOVirtual.Float(0f, 1f, moveDuration / 2, (v) => squash.Factor = v));
            seqMove.Insert(moveDuration / 2, DOVirtual.Float(1f, 0f, moveDuration / 2, (v) => squash.Factor = v));
        }
    }

    void HandleJump()
    {
        if (state != PlayerState.Idle) return;
        SwitchState(state, out state, PlayerState.Jump);
        seqJump = DOTween.Sequence().OnComplete(() => jumpUpDeform.Factor = 0).OnComplete(() => jumpDownDeform.Factor = 0);
        seqJump.Append(pivot.DOLocalJump(pos, jumpHeight, 1, jumpDuration).SetEase(jumpEase));
        seqJump.Join(DOVirtual.Float(0f, 1f, jumpDuration / 4, (v) => jumpUpDeform.Factor = v));
        seqJump.Insert(jumpDuration / 4, DOVirtual.Float(1f, 0f, jumpDuration / 4, (v) => jumpUpDeform.Factor = v));
        seqJump.Insert(jumpDuration / 2, DOVirtual.Float(0f, 1f, jumpDuration / 4, (v) => jumpDownDeform.Factor = v));
        seqJump.Insert(jumpDuration * 3 / 4, DOVirtual.Float(1f, 0f, jumpDuration / 4, (v) => jumpDownDeform.Factor = v));
        seqJump.InsertCallback(jumpDuration, () => SwitchState(state, out state, PlayerState.Idle));
    }

    void HandleSlide()
    {
        if (state != PlayerState.Idle) return;
        SwitchState(state, out state, PlayerState.Slide);
        seqJump = DOTween.Sequence().OnComplete(() => jumpUpDeform.Factor = 0).OnComplete(() => jumpDownDeform.Factor = 0);
        seqJump.Append(DOVirtual.Float(0f, -1f, slideDuration / 2, (v) => slideDeform.Factor = v));
        seqJump.Append(DOVirtual.Float(-1f, 0f, slideDuration / 2, (v) => slideDeform.Factor = v));
        seqJump.InsertCallback(slideDuration, () => SwitchState(state, out state, PlayerState.Idle));

    }

    void SwitchState(PlayerState pState, out PlayerState outState, PlayerState changeState)
    {
        CollidersList[(int)pState].gameObject.SetActive(false);
        CollidersList[(int)changeState].gameObject.SetActive(true);
        outState = changeState;
    }

    void BendThings(Material mat)
    {
        float TrackCurveParamX = Mathf.Lerp(-trackmgr.CurveAmplitudeX, trackmgr.CurveAmplitudeX, Mathf.PerlinNoise1D(trackmgr.elapsedTime * trackmgr.CurveFrequencyX));
        float TrackCurveParamY = Mathf.Lerp(-trackmgr.CurveAmplitudeY, trackmgr.CurveAmplitudeY, Mathf.PerlinNoise1D(TrackCurveParamX * trackmgr.CurveFrequencyY));
        mat.SetVector(curveAmount, new Vector4(TrackCurveParamX, TrackCurveParamY, 0f, 0f));
    }
    float _lastTriggerTime;
    void OnTriggerEnter(Collider other)
    {
        if(Time.time-_lastTriggerTime<0.2f) return;
        if (other)
        {
            if (other.tag=="Collectable")
            {
                feedbackImpact?.PlayFeedbacks();
                var c = other.GetComponentInParent<Collectable>();
                other.enabled=false;
                c?.Collect();

            }
            else if(other.tag=="Obstacle")
            {
                float a=trackmgr.scrollspeed;
                feedbackCrash?.PlayFeedbacks();
                GameManager.life--;
                other.enabled=false;
                if (GameManager.life<=0)
                {
                    GameManager.IsPlaying = false;
                }
            }
        }
        _lastTriggerTime=Time.time;
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
