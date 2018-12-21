using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    /// <summary>
    /// SO FAR THIS WORKS EXCEPT WHEN WALKING INTO A WALL FROM THE BACK
    /// </summary>

    public float turnSpeed;
    public float moveSpeed;
    private Vector3 moveDirection;
    public float height;
    public float heigtPadding;
    public LayerMask ground;
    public float maxGroundAngle = 150;
    public bool debug;
    public float gravityScale;

    private float gravDistDiff;
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

        if (!input.Equals(Vector2.zero))
        {
            CalculateDirection();
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
        if (Physics.Raycast(transform.position, -Vector3.up, out hitInfo, height + heigtPadding, ground))
        {

            // This is done to stop bouncing
            if (Vector3.Distance(transform.position, hitInfo.point) < height)
            {
                transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * height, 5 * Time.deltaTime);
            }
            grounded = true;

            // When gravity needs to act the height position must be stored
            initGravDist = transform.position.y;
        }
        else
        {
            grounded = false;
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

    // Transform players position
    void Move()
    {
        // If surface is not too steep
        if (groundAngle < maxGroundAngle)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
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
            Physics.Raycast(transform.position, -Vector3.up, out hitInfo, transform.position.y + 2, ground);

            // Prevent the player from jumping over the plane
            if (gravMoveDist.y < transform.position.y - hitInfo.distance)
            {
                gravMoveDist.y = height + transform.position.y - hitInfo.distance;
            }
            transform.position = gravMoveDist;
        }

    }

    // Add any debug checks here
    void DrawDebugLines()
    {
        if (debug)
        {
            Debug.DrawLine(transform.position, transform.position + transform.right * height * 4, Color.blue);
            Debug.DrawLine(transform.position, transform.position + moveDirection * height * 3, Color.green);
            Debug.DrawLine(transform.position, transform.position + -Vector3.up * hitInfo.distance, Color.black);

            Debug.Log(hitInfo.distance);
            Debug.Log(gravMoveDist.y);
            Debug.Log(grounded);

        }
    }
}