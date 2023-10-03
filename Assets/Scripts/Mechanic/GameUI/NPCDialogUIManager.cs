using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialogUIManager : MonoBehaviour {
  public static NPCDialogUIManager I;

  private GameObject container;
  private NPC npc;

  [SerializeField] private Image npcAvatar;
  [SerializeField] private TextMeshProUGUI npcNameText;
  [SerializeField] private Button dialogPanelContainerBtn;
  [SerializeField] private TextMeshProUGUI dialogPanelText;
  [SerializeField] private Button acceptQuestBtn;
  [SerializeField] private Button refuseQuestBtn;
  [SerializeField] private Button completeQuestBtn;

  public bool IsShow { get; private set; } = false;

  private int currentTextPointer;

  private void Awake() {
    I = this;

    container = transform.GetChild(0).gameObject;
    if (container != null) {
      container.SetActive(false);
    }
  }

  void Start() {
    NPCEvents.OnNPCInteractionStarted += Show;
    NPCEvents.OnNPCInteractionEnded += (NPC npc) => Hide();

    dialogPanelContainerBtn.onClick.AddListener(OnNextDialog);
    acceptQuestBtn.onClick.AddListener(OnAcceptQuestBtnClick);
    refuseQuestBtn.onClick.AddListener(OnRefuseBtnClick);
    completeQuestBtn.onClick.AddListener(OnCompleteQuestBtnClick);

    Hide();
  }

  // Update is called once per frame
  void Update() {

  }

  private void Show(NPC npc) {
    IsShow = true;
    this.npc = npc;

    container.SetActive(true);
    npcNameText.text = npc.data.Name;
    npcAvatar.sprite = npc.data.Sprite;
    acceptQuestBtn.gameObject.SetActive(false);
    refuseQuestBtn.gameObject.SetActive(false);
    completeQuestBtn.gameObject.SetActive(false);

    currentTextPointer = -1;
    OnNextDialog();
  }

  private void Hide() {
    IsShow = false;
    container.SetActive(false);
  }

  private void OnNextDialog() {
    currentTextPointer++;

    bool isQuest = IsQuest();
    if (isQuest) {
      if (npc.QuestAccepted) {
        if (npc.data.Quest.Completed) {
          if (currentTextPointer >= npc.data.Quest.QuestCompletedTexts.Length) {
            return;
          }
          if (currentTextPointer >= npc.data.Quest.QuestCompletedTexts.Length - 1) {
            completeQuestBtn.gameObject.SetActive(true);
          }
          dialogPanelText.text = npc.data.Quest.QuestCompletedTexts[currentTextPointer];
        }
        else {
          if (currentTextPointer >= npc.data.Quest.QuestAcceptedTexts.Length) {
            NPCEvents.PlayerInteractionWithNPCDone(npc);
            return;
          }
          dialogPanelText.text = npc.data.Quest.QuestAcceptedTexts[currentTextPointer];
        }
      }
      else {
        if (currentTextPointer >= npc.data.Quest.QuestBackStory.Length) {
          return;
        }
        if (currentTextPointer >= npc.data.Quest.QuestBackStory.Length - 1) {
          acceptQuestBtn.gameObject.SetActive(true);
          refuseQuestBtn.gameObject.SetActive(true);
        }
        dialogPanelText.text = npc.data.Quest.QuestBackStory[currentTextPointer];
      }
    }
    else {
      if (currentTextPointer >= npc.data.Dialogs.Length - 1) {
        NPCEvents.PlayerInteractionWithNPCDone(npc);
      }
      dialogPanelText.text = npc.data.Dialogs[currentTextPointer];
    }
  }

  private bool IsQuest() {
    return npc.data.Quest != null;
  }

  private void OnAcceptQuestBtnClick() {
    npc.PlayerAcceptedQuest();
    NPCEvents.PlayerInteractionWithNPCDone(npc);
  }

  private void OnRefuseBtnClick() {
    NPCEvents.PlayerInteractionWithNPCDone(npc);
  }

  private void OnCompleteQuestBtnClick() {
    QuestEvents.QuestDone(npc.data.Quest);
    NPCEvents.PlayerInteractionWithNPCDone(npc);
  }
}
