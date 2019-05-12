using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollowPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    public Vector3 offset;
    public float cameraSpeedScaler;
    private Vector3 currentCameraPosition;
    private Vector3 distError;


    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        currentCameraPosition = transform.position - offset;
        distError = player.position - currentCameraPosition;
        float smoothSpeed = Mathf.Pow(distError.magnitude,2)/100 * Time.deltaTime * cameraSpeedScaler;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        
    }
}
