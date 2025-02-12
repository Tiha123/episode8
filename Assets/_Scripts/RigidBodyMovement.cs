using UnityEngine;

public class RigidBodyMovement : MonoBehaviour
{
    float horz;
    float vert;
    bool jump;

    [SerializeField] float movespeed;
    [SerializeField] float rotatespeed;
    [SerializeField] float jumpforce;
    [SerializeField] float gravity;

    [SerializeField] Rigidbody rb;

    Transform camTransform;
    Vector3 move;

    bool isGrounded;

    void Start()
    {
        camTransform = Camera.main.transform;
    }

    void Update()
    {
        isGrounded = Physics.Raycast(transform.position + Vector3.up, Vector3.down, 1.3f);
        CustomGravity();
        inputKeboard();
        Movement();
        Rotate();
        Jump();
    }

    void inputKeboard()
    {
        horz = Input.GetAxisRaw("Horizontal");
        vert = Input.GetAxisRaw("Vertical");
        jump = Input.GetButtonDown("Jump");
        Vector3 forward = camTransform.forward;
        Vector3 right = camTransform.right;
        forward.y=0f;
        right.y=0f;
        // Vector3 move=new Vector3(horz,0f,vert);
        forward.Normalize();
        right.Normalize();
        
        move = (forward * vert + right * horz).normalized;
        // rb.AddForce(new Vector3(horz,0f,vert)*movespeed);

    }

    void Movement()
    {
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, move * movespeed, Time.deltaTime );
    }
    void Rotate()
    {
        if (move == Vector3.zero)
        {
            return;
        }
        Quaternion TargetRotation=Quaternion.LookRotation(move);
        rb.rotation = Quaternion.Slerp(rb.rotation, TargetRotation, Time.deltaTime * rotatespeed);
    }

    void CustomGravity()
    {
        rb.useGravity = isGrounded;
        if(!isGrounded)
        {
            rb.AddForce(Physics.gravity * gravity, ForceMode.Acceleration);
        }
    }

    void Jump()
    {
        if (jump&&isGrounded)
        {
            rb.AddExplosionForce(jumpforce, transform.position, 5f);
        }
    }
}
