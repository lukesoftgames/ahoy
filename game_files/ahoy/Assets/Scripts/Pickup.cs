using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float maxDist;
    public bool debug;
    public float height;
    public float heightPadding;
    public LayerMask ground;
    public float gravityScale;
    public float offsetFactor;

    private float debugVal;
    private float initGravDist;
    private float gravDistDiff;
    private Vector3 gravMoveDist;
    private bool grounded;
    private bool hold;
    private float dist;
    private RaycastHit hitInfo;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            if (!hold)
            {
                dist = Vector3.Distance(transform.position, player.position);
                if (dist < maxDist)
                {
                    transform.position = player.position + player.forward * offsetFactor;
                    hold = true;
                }
            }
            else
            {
                initGravDist = transform.position.y;
                hold = false;
            }
        }

        if (hold)
        {
            transform.position = player.position + player.forward * offsetFactor;
        }
        CheckGrounded();
        ApplyGravity();
        CheckDebug();
        
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

    // Apply force in y direction
    void ApplyGravity()
    {
        if (!grounded && !hold)
        {
            // Calculate how much the player has moved from the start of its fall
            gravDistDiff = Mathf.Abs(initGravDist - transform.position.y) + 1f;

            debugVal = Mathf.Sqrt(gravDistDiff) * Time.deltaTime * gravityScale;
            Debug.Log(debugVal);

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

    void CheckDebug()
    {
        if (debug)
        {
            Debug.DrawLine(player.position, player.position + player.forward * 4, Color.black);
        }
    }
}
