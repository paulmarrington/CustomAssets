#if UNITY_EDITOR && CustomAssets
using System;
using UnityEngine;
using UnityEngine.UI;

/// <inheritdoc />
/// <summary>
/// Sinple listener to react to a directly triggered event from a button press
/// </summary>
public sealed class DirectEventListenerSample : CustomAsset.Listener {
  [SerializeField] private Text textComponent;

  public override void OnTriggered() {
    textComponent.text = "Direct Event heard at " + DateTime.Now;
  }
}
#endif