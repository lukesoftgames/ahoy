using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Object
{
    [SerializeField]
    private float turnSpeed, moveSpeed, maxGroundAngle;

    private bool holding;
    private Vector3 moveDirection;
    private float currentAngle;
    private Vector2 input;
    private float angle;
    private float groundAngle;
    private Quaternion targetRotation;
    private float mag;
    private float mag1;
    private RaycastHit hitWall;

    [SerializeField]
    private float pickupRadius, offsetFactor, loadRadius;

    [SerializeField]
    private LayerMask itemLayer, cannonLayer;

    private Transform holdingItem;
    private Collider[] cannonColliders;
    private Collider[] itemColliders;

    private bool nearCannon;
    private bool nearItem;
    private Transform cannon;
    private float minDist;
    private Transform item;
    private float dist;
    private int minIdx;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    protected override void Update()
    {
        GetInput();
        base.Update();

        // Move
        if (!input.Equals(Vector2.zero))
        {
            CalculateDirection();
            CalculateCurrentDirection();
            CalculateMove();
            CalculateGroundAngle();
            Rotate();
            Move();

        }

        // Press A
        if (Input.GetButtonDown("Submit"))
        {
            InCannonRange();
            if (holding)
            {
                if (nearCannon)
                {
                    LoadCannon();
                }
                else
                {
                    Drop();
                }
            }
            else
            {
                InItemRange();
                if (nearItem)
                {
                    Pickup();
                }
                else if (nearCannon)
                {
                    UnloadCannon();
                }
            }
        }
    }

    void GetInput()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
    }

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

    void CalculateMove()
    {
        if (!base.GetGrounded())
        {
            moveDirection = transform.forward;
        }
        else
        {


            // Cross product of surface normal and players right direction. This will give the forward direction
            moveDirection = Vector3.Cross(transform.right, base.GetHitInfo().normal);

            // Scale magnitude to users input magnitude
            mag = moveDirection.magnitude;
            mag1 = new Vector3(input.x, 0f, input.y).magnitude;
            moveDirection = moveDirection * mag1 / mag;
        }
    }

    void CalculateGroundAngle()
    {
        if (!base.GetGrounded())
        {
            groundAngle = 90;
        }
        else
        {
            groundAngle = Vector3.Angle(base.GetHitInfo().normal, transform.forward);
        }
    }

    // Rotate player
    void Rotate()
    {
        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    // Transform players position
    void Move()
    {
        if (Physics.Raycast(transform.position, moveDirection, out hitWall, base.GetHeight(), GetGround()))
        {
            return;
        }
        // If surface is not too steep
        if (groundAngle < maxGroundAngle)
        {
            transform.position += (moveDirection * moveSpeed) * Time.deltaTime;
        }
    }

    void InCannonRange()
    {
        cannonColliders = Physics.OverlapSphere(transform.position, loadRadius, cannonLayer);
        if (cannonColliders.Length == 0)
        {
            nearCannon = false;
        }
        else
        {
            nearCannon = true;
        }
    }

    void InItemRange()
    {
        itemColliders = Physics.OverlapSphere(transform.position, pickupRadius, itemLayer);
        if (itemColliders.Length == 0)
        {
            nearItem = false;
        }
        else
        {
            nearItem = true;
        }

    }

    void LoadCannon()
    {
        cannon = cannonColliders[0].transform;
        if (!cannon.GetComponent<Cannon>().GetReadyToFire())
        {
            holdingItem.GetComponent<Object>().SetHeld(false);
            holdingItem.GetComponent<Object>().SetLoaded(true);

            holdingItem.position = new Vector3(-1000f, -1000f, -1000f);
            holdingItem.GetComponent<Renderer>().enabled = false;
            holdingItem.SetParent(cannon);
            cannon.GetComponent<Cannon>().SetReadyToFire(true);
            cannon.GetComponent<Cannon>().SetLoadedObject(holdingItem);

            holdingItem = null;
            holding = false;

        }
        else
        {
            Drop();
        }
    }

    void UnloadCannon()
    {
        cannon = cannonColliders[0].transform;
        if (cannon.GetComponent<Cannon>().GetReadyToFire())
        {
            holdingItem = cannon.GetComponent<Cannon>().GetLoadedObject();
            holdingItem.SetParent(transform);
            holdingItem.position = transform.position + transform.forward * offsetFactor;

            holdingItem.GetComponent<Object>().SetHeld(true);
            holdingItem.GetComponent<Object>().SetLoaded(false);

            cannon.GetComponent<Cannon>().SetLoadedObject(null);
            cannon.GetComponent<Cannon>().SetReadyToFire(false);

            holdingItem.GetComponent<Renderer>().enabled = true;

            holding = true;
        }
        else
        {
            Pickup();
        }
    }

    void Pickup()
    {
        itemColliders = Physics.OverlapSphere(transform.position, pickupRadius, itemLayer);
        if (itemColliders.Length == 0)
        {
            return;
        }

        FindNearestItem();
        holdingItem = itemColliders[minIdx].transform;
        holdingItem.parent = transform;
        holdingItem.position = transform.position + transform.forward * offsetFactor;
        holdingItem.GetComponent<Object>().SetHeld(true);

        holding = true;
    }

    void Drop()
    {
        holdingItem.parent = transform.parent;
        holdingItem.GetComponent<Object>().SetHeld(false);
        holdingItem = null;
        holding = false;
    }


    void FindNearestItem()
    {
        minDist = Mathf.Infinity;

        for (int i = 0; i < itemColliders.Length; i++)
        {
            item = itemColliders[i].transform;
            
            if (item.GetComponent<Object>().GetHeld())
            {
                Debug.Log("OOps");
                continue;
            }
            dist = Vector3.Distance(transform.position, item.position);

            if (dist < minDist)
            {
                minDist = dist;
                minIdx = i;
            }
        }
    }
}
