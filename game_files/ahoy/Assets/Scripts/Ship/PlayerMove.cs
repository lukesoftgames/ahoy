using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
    public ShipMovementController controller;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        controller.Turn(Input.GetAxis("Horizontal"));
    }
}
