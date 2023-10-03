using UnityEngine;

[CreateAssetMenu(fileName = "BodyArmorSO", menuName = "ScriptableObjects/BodyArmorSO")]
public class BodyArmorSO : ArmorSO {
  public override ArmorTypes ArmorType { get; } = ArmorTypes.BODY;
}
