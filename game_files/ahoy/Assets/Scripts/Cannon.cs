using UnityEngine;

public class Cannon : Object
{
    [SerializeField]
    private Transform endCannon;
<<<<<<< HEAD
=======

>>>>>>> 1207e6c9168fa51ee51032b31e06cd8093d5cd00


    private float bounceSpeed;
    private float initBounceDist;
    private bool readyToFire;
    private Transform loadedObject;

<<<<<<< HEAD
    private Animator anim;

    
=======

>>>>>>> 1207e6c9168fa51ee51032b31e06cd8093d5cd00

    // Start is called before the first frame update
    void Start()
    {
<<<<<<< HEAD
        anim = GetComponentInChildren<Animator>();
=======

>>>>>>> 1207e6c9168fa51ee51032b31e06cd8093d5cd00
    }

    public bool GetReadyToFire()
    {
        return readyToFire;
    }

    public Transform GetLoadedObject()
    {
        return loadedObject;
    }

<<<<<<< HEAD
    public Transform GetCannonEnd()
    {
        return endCannon;
=======
    public Vector3 GetCannonHeight()
    {
        return endCannon.position;
>>>>>>> 1207e6c9168fa51ee51032b31e06cd8093d5cd00
    }

    public void SetReadyToFire(bool inpReadyToFire)
    {
        readyToFire = inpReadyToFire;
        anim.SetBool("isReadyToFire", inpReadyToFire);
    }

    public void SetLoadedObject(Transform inpObject)
    {
        loadedObject = inpObject;
    }
}
