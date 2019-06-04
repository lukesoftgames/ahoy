using UnityEngine;

public class CannonballDamager : Damager {
    public override void onCollision(GameObject obj) {
        IDamageable damageable = obj.transform.parent.GetComponentInChildren<IDamageable>();

        if(damageable != null) {
            damageable.onDamage(this);
        }

        Destroy(gameObject);
    }
}