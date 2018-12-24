using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float turnSpeed;
    public float moveSpeed;
    private Vector3 moveDirection;
    public float height;
    public float heightPadding;
    public LayerMask ground;
    public float maxGroundAngle = 150;
    public bool debug;
    public float gravityScale;
    public float width;

    private Vector3 prevGroundMove;
    private Vector3 groundMove;
    private Transform prevGround;

    private float counter;
    private Vector3 prevScale;
    private float gravDistDiff;
    private float currentAngle;
    private Vector3 gravMoveDist;
    private Vector2 input;
    private float angle;
    private float groundAngle;
    private bool grounded;
    private Quaternion targetRotation;
    private float mag;
    private float mag1;
    private float initGravDist;
    private RaycastHit hitInfo;
    private RaycastHit hitGround;
    private RaycastHit hitWall;


    // Start is called before the first frame update
    void Start()
    {
        // If player not touching the ground initially gravity needs to be calculated
        initGravDist = transform.position.y;
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        GetInput();
        CheckGrounded();
        ApplyGravity();
        CalculateGroundMove();

        if (!input.Equals(Vector2.zero))
        {
            CalculateDirection();
            CalculateCurrentDirection();
            CalculateMove();
            CalculateGroundAngle();

            Rotate();
            Move();
        }

        DrawDebugLines();
    }

    // Get vector telling where the user wishes to go
    void GetInput()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
    }

    // Check if user is touching a ground surface
    void CheckGrounded()
    {

        // Produce raycast to ground
        if (Physics.Raycast(transform.position, -Vector3.up, out hitInfo, height + heightPadding, ground))
        {

            // This is done to stop bouncing
            if (Vector3.Distance(transform.position, hitInfo.point) < height)
            {
                transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * height, Time.deltaTime);
            }
            grounded = true;

            // When gravity needs to act the height position must be stored
            initGravDist = transform.position.y;
            Debug.Log(hitInfo.transform.name);
        }
        else
        {
            grounded = false;
            Debug.Log(counter);
            counter++;
        }

    }


    // Calculate movement vector
    void CalculateMove()
    {
        if (!grounded)
        {
            
            moveDirection = transform.forward;
        }
        else
        {

            // Cross product of surface normal and players right direction. This will give the forward direction
            moveDirection = Vector3.Cross(transform.right, hitInfo.normal);

            // Scale magnitude to users input magnitude
            mag = moveDirection.magnitude;
            mag1 = new Vector3(input.x, 0f, input.y).magnitude;
            moveDirection = moveDirection * mag1 / mag;         
        }
    }


    // Calculate the players angle with the ground
    void CalculateGroundAngle()
    {
        if (!grounded)
        {
            groundAngle = 90;
        }
        else
        {
            groundAngle = Vector3.Angle(hitInfo.normal, transform.forward);
        }
    }

    // Calculate angle of input direction
    void CalculateDirection()
    {
        angle = Mathf.Atan2(input.x, input.y);
        angle = Mathf.Rad2Deg * angle;
    }

    void CalculateCurrentDirection()
    {
        currentAngle = Mathf.Atan2(transform.forward.x, transform.forward.z);
        currentAngle = Mathf.Rad2Deg * currentAngle;
    }

    void CalculateGroundMove()
    {
       // groundMove = hitInfo.transform.position;
        if (grounded)
        {
            if (!hitInfo.transform.Equals(prevGround))
            {
                transform.parent = hitInfo.transform;
            }
            // transform.SetParent(hitInfo.transform, true);
            prevGround = hitInfo.transform;
        }

    }

    // Transform players position
    void Move()
    {
        if (Physics.Raycast(transform.position,moveDirection,out hitWall, height, ground))
        {
            //transform.position += 10*-heightPadding * transform.forward * Time.deltaTime;
            return;
        }
        // If surface is not too steep
        if (groundAngle < maxGroundAngle)
        {
            if (Input.GetButton("Submit"))
            {
                transform.position += (moveDirection * moveSpeed) * Time.deltaTime;

            }
            transform.position += (moveDirection * moveSpeed) * Time.deltaTime;
        }
    }

    // Rotate player
    void Rotate()
    {
        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    // Apply force in y direction
    void ApplyGravity()
    {
        if (!grounded)
        {
            // Calculate how much the player has moved from the start of its fall
            gravDistDiff = Mathf.Abs(initGravDist - transform.position.y) + 1f;

            // Calculate how much the player should move with gravitational acceleration
            gravMoveDist = transform.position + Physics.gravity * Mathf.Sqrt(gravDistDiff) * Time.deltaTime * gravityScale;
            Physics.Raycast(transform.position, -Vector3.up, out hitGround, transform.position.y + 2, ground);

            // Prevent the player from jumping over the plane
            if (gravMoveDist.y < hitGround.point.y + height)
            {
                gravMoveDist.y = hitGround.point.y +height;
            }
            transform.position = gravMoveDist;
        }

    }

    // Add any debug checks here
    void DrawDebugLines()
    {
        if (debug)
        {
            Debug.DrawLine(transform.position, transform.position + transform.forward * height * 4, Color.blue);
            Debug.DrawLine(transform.position, transform.position + moveDirection * height * 3, Color.green);

        }
    }
}