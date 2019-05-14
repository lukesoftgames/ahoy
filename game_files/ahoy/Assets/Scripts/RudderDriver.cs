using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RudderDriver : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody parentRB;

    private float rudderForce;

    public float maxRudderForce;

    public Transform turningForceTrans;
    
    private Vector3 turningForcePosition;
    void Start()
    {
        turningForcePosition = turningForceTrans.position;
    }

    // Update is called once per frame
    void Update()
    {
        float waveYPos = WaterController.current.GetWaveYPos(transform.position, Time.time);
        if (transform.position.y < waveYPos) {
            //apply force
            float dir = Input.GetAxis("Horizontal");
            if (dir != 0) {
                float t = Time.deltaTime;
                rudderForce += dir * t;
                transform.Rotate(new Vector3(0,dir * t,0));
            }
            Vector3 rudderForceVector = transform.right * rudderForce;
            
            parentRB.AddForceAtPosition(rudderForceVector*100f*-1, turningForcePosition);
        } else {
            if (rudderForce != 0) {
                rudderForce += Mathf.Sign(rudderForce) * -1 * Time.deltaTime * 100f;
            } 
        }
        parentRB.AddForceAtPosition(transform.forward*40000f, transform.position);

        
       
    }
}
