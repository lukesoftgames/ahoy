using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RudderDriver : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody parentRB;
    public Slider slider;
    private float rudderForce = 0;

    public float maxRudderForce;

    public Transform turningForceTrans;
    
    public float[] speed;

    public float[] turning;

    public int currentSpeed;
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
            //apply forc
            float dir = Input.GetAxis("Horizontal");
            float dampenedDir = dir * 0.01f;
            if (Mathf.Abs(rudderForce + dampenedDir) < maxRudderForce) {
                rudderForce += dampenedDir;
            }
            slider.value = rudderForce;
            parentRB.AddForceAtPosition(transform.right*turning[currentSpeed]*rudderForce, turningForcePosition);
            //transform.Rotate(new Vector3(0,dir*Time.deltaTime*10f,0));
        } 
        parentRB.AddForceAtPosition(transform.forward*speed[currentSpeed], transform.position);
        
        
       
    }
}
