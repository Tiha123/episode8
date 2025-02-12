
using System;
using UnityEngine;

public class QuaternionMovement : MonoBehaviour
{
    [SerializeField] float horz;
    [SerializeField] float vert;
    [SerializeField] Vector3 Direct;
    [SerializeField] bool IsJump;
    [SerializeField] float rotatespeed;
    [SerializeField] float movespeedX;
    [SerializeField] float scrollspeedY;
    [SerializeField] Camera cameraname;
    [SerializeField] GameObject Player;
    [SerializeField] float timeAnchor;
    [SerializeField] float jumpPower;
    [SerializeField] float jumpDuration;
    [SerializeField] float ChargeJump;
    [SerializeField] Vector3 JumpStartPosition;
    [SerializeField] LayerMask layermasks;
    [SerializeField] AnimationCurve jumpCurve;
    // void OnDrawGizmos()
    // {
    //     // Gizmos.color=Color.blue;
    //     // Gizmos.DrawRay(Vector3.zero, Vector3.forward*vert*3f);
    //     // Gizmos.color=Color.red;
    //     // Gizmos.DrawRay(Vector3.zero, Vector3.right*horz*3f);
    //     // Gizmos.color=Color.green;
    //     // Gizmos.DrawRay(Vector3.zero, (Vector3.right*horz+Vector3.forward*vert)*3f);
    //     Gizmos.color=Color.blue;
    //     Vector3 right=cameraname.transform.forward.normalized;
    //     right.y=0f;
    //     Gizmos.DrawRay(Vector3.zero, right*vert*3f);
    //     Gizmos.color=Color.red;
    //     Vector3 forward=cameraname.transform.right.normalized;
    //     forward.y=0f;
    //     Gizmos.DrawRay(Vector3.zero, forward*horz*3f);
    // }
    void Start()
    {

    }
    void FixedUpdate()// 1프레임당 여러번
    {
        horz = Input.GetAxisRaw("Horizontal");
        vert = Input.GetAxisRaw("Vertical");
    }
    void Update()// 매 프레임당 1번
    {
        DegreeCaculate(out Direct);
        UpdateRotationByCamera();
        UpdateMoveByCamera();
        UpdateJump();
        // Debug.DrawRay(transform.position,Direct*500f,Color.green);
    }
    // void UpdateMove()
    // {
    //     Vector3 movedir=new Vector3(horz,0f,vert)*Time.deltaTime*movespeed;
    //     transform.position+=movedir;

    //     transform.Translate(horz*movespeed*Time.deltaTime,0,vert*movespeed*Time.deltaTime);
    // }
    void UpdateMoveByCamera()
    {
        // transform.position+=movedir;
        // Player.transform.Translate();
        // Player.transform.position=Vector3.MoveTowards(Player.transform.position, Player.transform.position+Direct*50f,movespeed*Time.deltaTime);

        transform.position = Vector3.Lerp(transform.position, transform.position + Direct, Time.deltaTime);

    }

    // void UpdateRotation()
    // {
    //     System.Threading.Thread.Sleep(10);
    //     horz=Input.GetAxis("Horizontal");
    //     vert=Input.GetAxis("Vertical");
    //     // horz=Input.GetAxisRaw("Horizontal");
    //     // vert=Input.GetAxisRaw("Vertical");
    //     // Debug.Log($"{horz}");
    //     // Debug.Log($"{vert}");
    //     Vector3 direct=Vector3.right*horz+Vector3.forward*vert;
    //     if(!Input.GetButton("Horizontal") && !Input.GetButton("Vertical"))
    //     {
    //         return;
    //     }
    //     Quaternion lookrotation=Quaternion.LookRotation(direct);
    //     transform.rotation=Quaternion.RotateTowards(transform.rotation,lookrotation,rotatespeed*Time.deltaTime);
    // }

    void UpdateRotationByCamera()
    {
        // horz=Input.GetAxisRaw("Horizontal");
        // vert=Input.GetAxisRaw("Vertical");
        // Debug.Log($"{horz}");
        // Debug.Log($"{vert}");

        if (Direct==new Vector3(0f,0f,0f))
        {
            return;
        }

        Quaternion lookrotation = Quaternion.LookRotation(Direct);

        // float yAngle=lookrotation.y;
        // Player.transform.rotation=Quaternion.Euler(0f,yAngle,0f);

        Player.transform.rotation = Quaternion.RotateTowards(Player.transform.rotation, lookrotation, rotatespeed * Time.deltaTime);
    }

    void UpdateJump()
    {

        if (Input.GetButtonDown("Jump") && IsJump == false)
        {
            timeAnchor = Time.time;
            JumpStartPosition = transform.position;
        }
        else if (Input.GetButtonUp("Jump") && IsJump == false)
        {
            ChargeJump = Math.Clamp(Time.time - timeAnchor, 0.7f, 3f);
            timeAnchor = Time.time;
            JumpStartPosition = transform.position;
            IsJump = true;
        }

        if (IsJump == true)
        {
            float percent = (Time.time - timeAnchor) / jumpDuration;
            if (percent <= 1f)
            {
                //float jumpHeight = (percent - percent * percent) * jumpPower*ChargeJump;
                float jumpHeight = jumpCurve.Evaluate(percent) * jumpPower * ChargeJump;
                transform.position = new Vector3(transform.position.x, JumpStartPosition.y + jumpHeight, transform.position.z);
            }
            else
            {
                RayCastSet();
                ChargeJump = 0f;
                IsJump = false;
            }
        }
        //포물선 방정식
        // 1: y=(x-x*x)
        // 2: y=x*(1-x)
        // y는 점프 높이, x는 시간변화량=퍼센트(진행도)
        // Time.deltaTIme: 1프레임당 걸린 시간
        // Time.time: 현재 시간
    }

    void DegreeCaculate(out Vector3 DirectVector)
    {
        Vector3 forward;

        if (cameraname.transform.rotation.x > 85f || cameraname.transform.rotation.x < 95f)
        {
            forward = cameraname.transform.up.normalized * vert;
        }
        else
        {
            forward = cameraname.transform.forward.normalized * vert;
        }

        Vector3 right = cameraname.transform.right.normalized * horz;

        DirectVector = forward + right;

        DirectVector.y = 0;

        DirectVector = DirectVector.normalized;

        DirectVector.z*=scrollspeedY;

        DirectVector.x*=movespeedX;
    }

    void RayCastSet()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, -transform.up, out hit, 1000.0f, layermasks);
        if (hit.point.y > 0f)
        {
            transform.position = new Vector3(transform.position.x, hit.point.y + Player.transform.localScale.y / 2, transform.position.z);
        }
    }
}
