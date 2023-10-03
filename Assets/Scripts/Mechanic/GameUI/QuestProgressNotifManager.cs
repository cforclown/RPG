using System.Collections;
using TMPro;
using UnityEngine;

public class QuestProgressNotifManager : MonoBehaviour {
  private GameObject container;
  private TextMeshProUGUI progressText;

  private void Awake() {
    QuestEvents.OnQuestProgress += Show;
  }

  // Start is called before the first frame update
  void Start() {
    container = transform.GetChild(0).gameObject;
    progressText = container.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

    container.SetActive(false);
  }

  private void Show(QuestGoal questGoal) {
    progressText.color = new Color(255, 255, 255, 1f);
    progressText.text = questGoal.GetProgressText();
    container.SetActive(true);
    StopCoroutine(FadeAnim());
    StartCoroutine(FadeAnim());
  }

  private IEnumerator FadeAnim() {
    float a = progressText.color.a;
    yield return new WaitForSeconds(0.0001f);
    while (a > 0f) {
      a -= 0.5f * Time.deltaTime;
      progressText.color = new Color(255, 255, 255, a);
      yield return new WaitForSeconds(0.0001f);
    }
    container.SetActive(false);
  }
}
