using UnityEngine;

public sealed class DirectEventListenerSample : Events.BaseListener {
  public override void OnTriggered(Events.Listener listener) {
    Debug.Log("Direct Event heard and responded to");
  }
}