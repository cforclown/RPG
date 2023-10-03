using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
  [SerializeField] Button playBtn;

  void Start() {
    playBtn.onClick.AddListener(OnPlayBtnClick);
  }

  void OnPlayBtnClick() {
    SceneManager.LoadScene("Main");
  }
}
