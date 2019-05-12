using UnityEngine;

public class Cannon : Object {
    [SerializeField] private Transform endCannon;

    private bool readyToFire;
    private Transform loadedObject;

    public bool GetReadyToFire() {
        return readyToFire;
    }

    public Transform GetLoadedObject() {
        return loadedObject;
    }

    public Vector3 GetCannonHeight() {
        return endCannon.position;
    }

    public void SetReadyToFire(bool inpReadyToFire) {
        readyToFire = inpReadyToFire;
    }

    public void SetLoadedObject(Transform inpObject) {
        loadedObject = inpObject;
    }
}