// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using Askowl;
using CustomAsset.Mutable;
using UnityEngine;

namespace CustomAsset {
  /// <a href="http://bit.ly/2CwSVl4">Float CustomAsset contains a float value which can be connected directly to OnValueChange callbacks in UI slider and scrollbar components. Connect it to event listeners to interact with components such as Animation, Text or Unity. Or add listeners to your own classes with Register(this)</a>
  [CreateAssetMenu(menuName = "Custom Assets/Mutable/GameObject")]
  public class GameObject : OfType<UnityEngine.GameObject> {
    /// <a href="http://bit.ly/2CwSVl4">For safe(ish) access to the contents field</a>
    public override UnityEngine.GameObject Value {
      get => (base.Value != default) ? base.Value : NoGameObject();
      set => Set(value);
    }

    private UnityEngine.GameObject NoGameObject() {
      Debug.LogError("Use menu Component/Custom Asset Game Object Connector and add this asset reference");
      return Objects.CreateGameObject("Not assigned in Inspector");
    }
  }
}