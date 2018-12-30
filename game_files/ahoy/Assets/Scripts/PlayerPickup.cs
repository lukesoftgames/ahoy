using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{

    public bool holding;

    [SerializeField]
    private float pickupRadius, offsetFactor, loadRadius;

    [SerializeField]
    private LayerMask itemLayer, cannonLayer;

    [SerializeField]
    private Transform holdingItem;
    
    private Transform cannon;
    private bool nearCannon;
    private Transform item;
    private Transform loadItem;
    private float dist;
    private float minDist = Mathf.Infinity;
    private int minIdx;
    private Collider[] itemColliders;
    private Collider[] cannonColliders;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            InCannonRange();
            if (holding)
            {
                if (nearCannon)
                {
                    Debug.Log("Near Cannon");
                    LoadCannon();
                    //Drop();
                }
                else
                {
                    Drop();
                }
            }
            else
            {
                if (nearCannon)
                {
                    UnloadCannon();
                }
                else
                {
                    Pickup();
                }
            }
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
        holdingItem.GetComponent<Item>().held = true;
        holding = true;
    }

    void UnloadCannon()
    {
        cannon = cannonColliders[0].transform;
        if (cannon.GetComponent<Cannon>().loaded)
        {
            holdingItem = cannon.GetComponent<Cannon>().loadedItem;
            holdingItem.parent = transform;
            holdingItem.position = transform.position + transform.forward * offsetFactor;
            holdingItem.GetComponent<Item>().held = true;
            holdingItem.GetComponent<Item>().loaded = false;
            cannon.GetComponent<Cannon>().loadedItem = null;
            cannon.GetComponent<Cannon>().loaded = false;
            holdingItem.GetComponent<Renderer>().enabled = true;
            holding = true;
        }
        else
        {
            Pickup();
        }
    }

    void LoadCannon()
    {
        cannon = cannonColliders[0].transform;
        if (!cannon.GetComponent<Cannon>().loaded)
        {
            loadItem = holdingItem;
            Drop();
            loadItem.GetComponent<Item>().loaded = true;
            loadItem.position = new Vector3(-1000f, -1000f, -1000f);
            loadItem.GetComponent<Renderer>().enabled = false;
            cannon.GetComponent<Cannon>().loaded = true;
            cannon.GetComponent<Cannon>().loadedItem = loadItem;

        }
        else
        {
            Drop();
        }
    }

    void FindNearestItem()
    {
        minDist = Mathf.Infinity;

        for (int i = 0; i < itemColliders.Length; i++)
        {
            item = itemColliders[i].transform;
            dist = Vector3.Distance(transform.position, item.position);

            if (dist < minDist)
            {
                minDist = dist;
                minIdx = i;
            }
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

    void Drop()
    {
        holdingItem.parent = transform.parent;
        holdingItem.GetComponent<Item>().held = false;
        holdingItem = null;
        holding = false;
    }
}
