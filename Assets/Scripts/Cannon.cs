using UnityEngine;

public class Cannon : Object
{
    [SerializeField]
    private Transform endCannon;

    private float bounceSpeed;
    private float initBounceDist;
    private bool readyToFire;
    private Transform loadedObject;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public bool GetReadyToFire()
    {
        return readyToFire;
    }

    public Transform GetLoadedObject()
    {
        return loadedObject;
    }

    public Transform GetCannonEnd()
    {
        return endCannon;
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
