using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatsPanelManager : MonoBehaviour {
  public static CharacterStatsPanelManager I;

  [SerializeField] private TMP_Text playerNameText;
  [SerializeField] private TMP_Text statsPointText;
  [SerializeField] private TMP_Text strengthValText;
  [SerializeField] private Button increaseStrengthBtn;
  [SerializeField] private TMP_Text agilityValText;
  [SerializeField] private Button increaseAgilityhBtn;
  [SerializeField] private TMP_Text intelligenceValText;
  [SerializeField] private Button increaseIntelligenceBtn;
  [SerializeField] private TMP_Text hpValText;
  [SerializeField] private TMP_Text manaValText;
  [SerializeField] private TMP_Text armorValText;
  [SerializeField] private TMP_Text dmgValText;

  private void Awake() {
    if (I == null) {
      I = this;
    }

    increaseStrengthBtn.onClick.AddListener(() => {
      OnIncreaseStatsClick("STRENGTH");
    });
    increaseAgilityhBtn.onClick.AddListener(() => {
      OnIncreaseStatsClick("AGILITY");
    });
    increaseIntelligenceBtn.onClick.AddListener(() => {
      OnIncreaseStatsClick("INTELLIGENCE");
    });
  }

  public void SetPlayerStats(Character stats) {
    try {
      playerNameText.text = stats.Name;
      if (stats.StatsPoint > 0) {
        statsPointText.gameObject.SetActive(true);
        statsPointText.text = stats.StatsPoint.ToString();
        increaseStrengthBtn.gameObject.SetActive(true);
        increaseAgilityhBtn.gameObject.SetActive(true);
        increaseIntelligenceBtn.gameObject.SetActive(true);
      }
      else {
        statsPointText.gameObject.SetActive(false);
        increaseStrengthBtn.gameObject.SetActive(false);
        increaseAgilityhBtn.gameObject.SetActive(false);
        increaseIntelligenceBtn.gameObject.SetActive(false);
      }
      strengthValText.text = stats.Strength.ToString();
      agilityValText.text = stats.Agility.ToString();
      intelligenceValText.text = stats.Intelligence.ToString();
      hpValText.text = stats.MaxHP.ToString();
      manaValText.text = stats.MaxMP.ToString();
      armorValText.text = stats.Armor.ToString();
      dmgValText.text = stats.UnarmedDamage.ToString();
    }
    catch (Exception exc) {
      Logger.Err(this.name, exc.ToString());
    }
  }

  public void OnIncreaseStatsClick(string statsName) {
    try {
      switch (statsName) {
        case "STRENGTH":
          Player.I.Character.IncreaseStrength();
          break;
        case "AGILITY":
          Player.I.Character.IncreaseAgility();
          break;
        case "INTELLIGENCE":
          Player.I.Character.IncreaseIntelligence();
          break;
        default:
          break;
      }
      SetPlayerStats(Player.I.Character.Stats);
    }
    catch (Exception e) {
      Debug.LogWarning(e.Message);
    }
  }
}
