using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateShipModel : MonoBehaviour
{
    public float rotationMax;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void RotateModel(Vector3 rotate) {
        Debug.Log(rotate);
       
        transform.Rotate(rotate);
        
           
    
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
