#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class DirectEventListenerSample : CustomAsset.Listener {
  [SerializeField] private Text textComponent;

  public override void OnTriggered() {
    textComponent.text = "Direct Event heard at " + DateTime.Now;
  }
}
#endif