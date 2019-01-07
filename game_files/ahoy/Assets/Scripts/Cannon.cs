using UnityEngine;

public class Cannon : Object
{

    private bool readyToFire;
    private Transform loadedObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool GetReadyToFire()
    {
        return readyToFire;
    }

    public Transform GetLoadedObject()
    {
        return loadedObject;
    }

    public void SetReadyToFire(bool inpReadyToFire)
    {
        readyToFire = inpReadyToFire;
    }

    public void SetLoadedObject(Transform inpObject)
    {
        loadedObject = inpObject;
    }
}
