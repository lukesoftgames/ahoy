using UnityEngine;

public class Damager : MonoBehaviour {
    public virtual void OnCollisionEnter(Collision collision) {
        onCollision(collision.gameObject);
    }

    public virtual void onCollision(GameObject obj) {

    }
}