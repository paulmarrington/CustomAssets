// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;

namespace CustomAsset.Constant {
  /// <a href="http://bit.ly/2CwSS8S">Float CustomAsset contains a float value which can be connected directly to OnValueChange callbacks in UI slider and scrollbar components. Connect it to event listeners to interact with components such as Animation, Text or Unity. Or add listeners to your own classes with Register(this)</a><inheritdoc />
  [CreateAssetMenu(menuName = "Custom Assets/Constant/Float")]
  public sealed class Float : OfType<float> { }
}