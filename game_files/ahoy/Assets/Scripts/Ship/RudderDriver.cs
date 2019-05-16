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
    
    public float rudderDeadZone;

    public float[] speed;

    public float[] turning;
    public GameObject boat;

    private BoatPhysics boatPhysics;
    public int currentSpeed;
    private Vector3 turningForcePosition;

    void Start()
    {
        turningForcePosition = turningForceTrans.position;
        boatPhysics = boat.GetComponent<BoatPhysics>();
    }

    // Update is called once per frame
    void Update()
    {
        float waveYPos = WaterController.current.GetWaveYPos(transform.position, Time.time);
        if (transform.position.y < waveYPos) {
            //apply forc
            float dir = Input.GetAxis("Horizontal");
            float dampenedDir = dir * 0.01f * -1;
            if (Mathf.Abs(rudderForce + dampenedDir) < maxRudderForce) {
                rudderForce += dampenedDir;
            } 
            slider.value = rudderForce*-1;
            if (Mathf.Abs(rudderForce) > rudderDeadZone) {
                parentRB.AddForceAtPosition(transform.right*turning[currentSpeed]*rudderForce, turningForcePosition);
            }
        } 
        if (Input.GetKeyDown(KeyCode.W)) {
            if (currentSpeed < speed.Length) {
                currentSpeed++;
                //boatPhysics.centerOfMass = new Vector3(0,0,currentSpeed*0.5f);
                turningForceTrans.position = new Vector3(0,0,currentSpeed+1f);
            } 
        } 
        if (Input.GetKeyDown(KeyCode.S)) {
            if (currentSpeed > 0) {
                currentSpeed--; 
                //boatPhysics.centerOfMass = new Vector3(0,0,currentSpeed*0.5f);
              
            }
        }
        parentRB.AddForceAtPosition(transform.forward*speed[currentSpeed], transform.position);
        
        
       
    }
}
