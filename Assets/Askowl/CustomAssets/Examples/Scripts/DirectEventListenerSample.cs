#if UNITY_EDITOR && CustomAssets
using System;
using Askowl;
using CustomAsset.Mutable;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable MissingXmlDoc

public sealed class DirectEventListenerSample : ListenerComponent {
  [SerializeField] private Text textComponent = default;

  protected override void OnChange(Emitter _) => textComponent.text = "Direct Event heard at " + DateTime.Now;
}
#endif