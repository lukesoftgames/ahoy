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

    public Transform GetCannonEnd() {
        return endCannon;
    }

    public void SetReadyToFire(bool inpReadyToFire) {
        readyToFire = inpReadyToFire;
    }

    public void SetLoadedObject(Transform inpObject) {
        loadedObject = inpObject;
    }
}