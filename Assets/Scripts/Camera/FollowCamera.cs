using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform CameraSlot;
    float camSens = 0.1f; //How sensitive it with mouse
    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        //Cursor.lockState = CursorLockState
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = CameraSlot.position;
        transform.eulerAngles = (new Vector3(0,CameraSlot.eulerAngles.y,0));
        
    }
}
