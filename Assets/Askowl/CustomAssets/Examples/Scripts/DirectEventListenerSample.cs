#if UNITY_EDITOR && CustomAssets
using System;
using CustomAsset.Mutable;
using UnityEngine;
using UnityEngine.UI;

/// <a href="">Simple listener to react to a directly triggered event from a button press</a> //#TBD#// <inheritdoc />
public sealed class DirectEventListenerSample : ListenerComponent {
  [SerializeField] private Text textComponent;

  /// <inheritdoc />
  protected override void OnChange() {
    textComponent.text = "Direct Event heard at " + DateTime.Now;
  }
}
#endif