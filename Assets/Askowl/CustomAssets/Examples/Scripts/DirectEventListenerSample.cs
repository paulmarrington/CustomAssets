#if UNITY_EDITOR && CustomAssets
using System;
using UnityEngine;
using UnityEngine.UI;
using String = CustomAsset.String;

/// <inheritdoc />
/// <summary>
/// Sinple listener to react to a directly triggered event from a button press
/// </summary>
//public sealed class DirectEventListenerSample : CustomAsset.ListenerBehaviour {
//  [SerializeField] private Text textComponent;
//
//  protected override void OnChange(string memberName) {
//    textComponent.text = "Direct Event heard at " + DateTime.Now + " for " + memberName;
//  }
//}
public sealed class DirectEventListenerSample : CustomAsset.ListenerBehaviour {
  [SerializeField] private Text textComponent;

  /// <inheritdoc />
  protected override void OnChange(string memberName) {
    textComponent.text = "Direct Event heard at " + DateTime.Now + " for " + memberName;
  }
}
#endif