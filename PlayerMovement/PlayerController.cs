using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Var
    [SerializeField] private float movespeed;
    [SerializeField] private float walkspeed;
    [SerializeField] private float runspeed;

    private Vector3 moveDir;
    private Vector3 velocity;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;

    [SerializeField] private float jumpStrength;
    [SerializeField] private bool canJump;

    //Ref
    private CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move() {

        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance+(controller.height/2), groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");
        moveDir = new Vector3(moveX, 0, moveZ);
        moveDir = transform.TransformDirection(moveDir);

        if (isGrounded)
        {
            if (!Input.GetKey(KeyCode.Space)) { canJump = true; }
            if (moveDir != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                    Walk();
            }
            else if (moveDir != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                    Run();
            }
            else
            {
                    Idle();
            }

            moveDir *= movespeed;
            velocity.x = 0;
            velocity.z = 0;
        }

        if (Input.GetKey(KeyCode.Space) && canJump)
        {
            Jump();
        }

        controller.Move(moveDir * Time.deltaTime);

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Idle()
    {

    }

    private void Walk() 
    {
        movespeed = walkspeed;
    }

    private void Run() 
    {
        movespeed = runspeed;
    }

    private void Jump()
    {
        velocity.x = moveDir.x;
        velocity.y = jumpStrength;
        velocity.z = moveDir.z;
        canJump = false;
    }
    
}
