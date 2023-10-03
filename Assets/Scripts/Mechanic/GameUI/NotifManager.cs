using System.Collections;
using TMPro;
using UnityEngine;

public class NotifManager : MonoBehaviour {
  private GameObject container;
  private TextMeshProUGUI text;

  private void Awake() {
    QuestEvents.OnQuestFinished += DisplayQuestFinishedNotif;
  }

  // Start is called before the first frame update
  void Start() {
    container = transform.GetChild(0).gameObject;
    text = container.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

    container.SetActive(false);
  }

  private void DisplayQuestFinishedNotif(QuestSO quest) {
    text.color = new Color(255, 255, 255, 1f);
    text.text = string.Format(
      "Quest <color=#ffa200>{0}</color> completed\nReward <color=#ffa200>{1}</color> Exp",
      quest.QuestTitle,
      quest.ExpReward
    );
    container.SetActive(true);
    StopCoroutine(FadeAnim());
    StartCoroutine(FadeAnim());
  }

  private IEnumerator FadeAnim() {
    float a = text.color.a;
    yield return new WaitForSeconds(0.0001f);
    while (a > 0f) {
      a -= 0.5f * Time.deltaTime;
      text.color = new Color(255, 255, 255, a);
      yield return new WaitForSeconds(0.0001f);
    }
    container.SetActive(false);
  }
}
