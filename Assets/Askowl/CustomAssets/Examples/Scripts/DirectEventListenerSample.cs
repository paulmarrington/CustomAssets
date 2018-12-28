#if UNITY_EDITOR && CustomAssets
using System;
using CustomAsset.Mutable;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable MissingXmlDoc

public sealed class DirectEventListenerSample : ListenerComponent {
  [SerializeField] private Text textComponent = default;

  protected override void OnChange() => textComponent.text = "Direct Event heard at " + DateTime.Now;
}
#endif