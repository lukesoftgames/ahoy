using UnityEngine;

public class Item : MonoBehaviour
{
    public float height;
    public float heightPadding;
    public LayerMask ground;
    public bool held;
    public bool loaded;
    public float gravityScale;

    private Vector3 gravMoveDist;
    private float gravDistDiff;
    private float initGravDist;
    private bool grounded;
    private RaycastHit hitInfo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!held)
        {
            CheckGrounded();
            ApplyGravity();
        }
        else
        {
            initGravDist = transform.position.y;
        }
    }

    void CheckGrounded()
    {
        // Produce raycast to ground
        if (Physics.Raycast(transform.position, -Vector3.up, out hitInfo, height + heightPadding, ground))
        {

            // This is done to stop bouncing
            if (Vector3.Distance(transform.position, hitInfo.point) < height)
            {
                transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * height, 5 * Time.deltaTime);
            }
            grounded = true;

            // When gravity needs to act the height position must be stored
        }
        else
        {
            grounded = false;
        }

    }

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
            if (gravMoveDist.y < hitInfo.point.y + height)
            {
                gravMoveDist.y = height + hitInfo.point.y;
            }
            transform.position = gravMoveDist;
        }

    }
}
