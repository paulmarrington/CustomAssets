#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;

public sealed class DirectEventListenerSample : Events.BaseListener {
  [SerializeField] private Text textComponent;

  public override void OnTriggered(Events.Listener listener) {
    textComponent.text = "Direct Event heard and responded to";
  }
}
#endif