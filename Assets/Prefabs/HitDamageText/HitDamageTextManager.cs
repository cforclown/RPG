using UnityEngine;

public class HitDamageTextManager : MonoBehaviour {
  [SerializeField] private GameObject hitDamageTextPrefab;

  private void Start() {
    CombatEvents.OnAttacktHit += DisplayHitDamage;
  }

  private void DisplayHitDamage(Transform posRef, int damage) {
    GameObject hitDamageTextObj = Instantiate(hitDamageTextPrefab, transform);
    HitDamageText hitDamageText = hitDamageTextObj.GetComponent<HitDamageText>();
    hitDamageText.Init(damage, posRef);
  }
}
