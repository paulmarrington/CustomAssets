using CustomAsset;
using UnityEngine;

public sealed class DirectEventListenerSample : BaseEventListener {
  public override void OnEventRaised(IEventListener listener) {
    Debug.Log("Direct Event heard and responded to");
  }
}