using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    // view in editor
    [SerializeField]
    private float height, width;
    [SerializeField]
    private float heightPadding;
    [SerializeField]
    private float gravityScale;
    [SerializeField]
    private LayerMask ground;

    private Vector3 gravMoveDist;
    private float gravDistDiff;
    private float initGravDist;
    private bool grounded;
    private bool held;
    private bool loaded;
    private bool fired;
    private int power = 300;
    private Vector3 initForward;
    private RaycastHit hitInfo;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!fired)
        {
            Gravity();
        }
        else
        {
            Fire();
        }
    }


    protected void Fire()
    {
        transform.position = transform.position + initForward*power*Time.deltaTime;
    }

    protected void Gravity()
    {
        if (!held)
        {
            grounded = CheckGrounded();
            ApplyGravity();
        }
        else
        {
            initGravDist = transform.position.y;
        }
    }
    bool CheckGrounded()
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

        return grounded;

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

    public bool GetGrounded()
    {
        return grounded;
    }

    protected RaycastHit GetHitInfo()
    {
        return hitInfo;
    }

    protected float GetHeight()
    {
        return height;
    }

    protected LayerMask GetGround()
    {
        return ground;
    }

    public bool GetHeld()
    {
        return held;
    }

    public void SetHeld(bool inpHeld)
    {
        held = inpHeld;
    }

    public void SetLoaded(bool inpLoaded)
    {
        loaded = inpLoaded;
    }

    public void SetFired(bool inpFired)
    {
        fired = inpFired;
    }

    public void SetForward(Vector3 inpForward)
    {
        initForward = inpForward;
    }
}
