using UnityEngine;

public class ShipDamage : MonoBehaviour, IDamageable {
    private bool isDamaged;
    [SerializeField] private GameObject damagedObj;
    [SerializeField] private GameObject undamagedObj;
    private ShipDamageController controller;

    public void onDamage(Damager damager) {
        if(!isDamaged) {
            isDamaged = true;
            undamagedObj.SetActive(false);
            damagedObj.SetActive(true);

            controller.shipDamaged(this);
        }
    }

    public void setController(ShipDamageController controller) {
        this.controller = controller;
    }
}