public class NPCEvents {
  public delegate void NPCEventHandler(NPC npc);
  public static event NPCEventHandler OnNPCInteractionStarted;
  public static event NPCEventHandler OnNPCInteractionEnded;
  public static event NPCEventHandler OnPlayerAcceptNPCQuest;

  public static void PlayerInteractWithNPC(NPC npc) {
    if (OnNPCInteractionStarted == null) {
      return;
    }

    OnNPCInteractionStarted(npc);
  }

  public static void PlayerInteractionWithNPCDone(NPC npc) {
    if (OnNPCInteractionEnded == null) {
      return;
    }

    OnNPCInteractionEnded(npc);
  }

  public static void PlayerAcceptNPCQuest(NPC npc) {
    if (OnPlayerAcceptNPCQuest == null) {
      return;
    }

    OnPlayerAcceptNPCQuest(npc);
  }
}
