using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ShipMovementController : MonoBehaviour
{
   
    public RotateShipModel shipModel;
    private float bearing;
    [Header("Game-Move Parameters")]
    public float maxSpeed;
    private float currentSteeringForce = 0;
    public float maxSteeringForce;
    
    public float[] speed;
    public float[] turning; 

    public int currentSpeed;
    
    /*
        Turn the ship to the target
    */
    public void Turn(float dir) 
    {  
        Debug.Log(currentSteeringForce);
        if (dir != 0) {
            if (Mathf.Abs(currentSteeringForce) <  turning[currentSpeed]) {
                currentSteeringForce += dir*Time.deltaTime;
            }
        } else {
                if (Mathf.Abs(currentSteeringForce) < 0.01) {
                    currentSteeringForce = 0;
                } else {
                    if (currentSteeringForce < 0) {
                         currentSteeringForce += Time.deltaTime/10;
                    } else {
                         currentSteeringForce -= Time.deltaTime/10;
                    }
                }
        }
        Vector3 rotationVector = new Vector3(0, currentSteeringForce, 0);
        transform.Rotate(rotationVector);
        shipModel.RotateModel(new Vector3(currentSteeringForce*-1,0,0));
        Step();
    } 
    void Step() {
        transform.position = transform.position + transform.right * Time.deltaTime * speed[currentSpeed];
    }
    // Start is called before the first frame update
    void Start()
    {
      
    }
    // Update is called once per frame
    void Update()
    {  
           
    }
}
