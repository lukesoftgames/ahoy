using UnityEngine;

public class ShipDamageController : MonoBehaviour {
    private int damagedSections = 0;
    [SerializeField] private int damagedToSink;
    private ShipDamage[] shipDamages;

    private void Awake() {
        shipDamages = GetComponentsInChildren<ShipDamage>();

        foreach(ShipDamage damage in shipDamages) {
            damage.setController(this);
        }
    }

    public void shipDamaged(ShipDamage damage) {
        damagedSections++;

        if(damagedSections >= damagedToSink) {
            //Sink ship
        }
    }
}