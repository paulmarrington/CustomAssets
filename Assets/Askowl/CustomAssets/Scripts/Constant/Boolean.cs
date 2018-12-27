// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;

namespace CustomAsset.Constant {
  /// <a href="http://bit.ly/2CwSS8S">Boolean custom asset. Triggers event when changing from true to false or false to true</a> <inheritdoc />
  [CreateAssetMenu(menuName = "Custom Assets/Constant/Boolean")]
  public sealed class Boolean : OfType<bool> { }
}