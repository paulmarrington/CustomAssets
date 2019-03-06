#if !ExcludeAskowlTests
using System;
using Askowl;
using CustomAsset.Mutable;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable MissingXmlDoc
namespace Askowl.CustomAssets.Examples {

  public sealed class DirectEventListenerSample : ListenerComponent {
    [SerializeField] private Text textComponent = default;

    protected override void OnChange(Emitter _) => textComponent.text = "Direct Event heard at " + DateTime.Now;
  }
}
#endif