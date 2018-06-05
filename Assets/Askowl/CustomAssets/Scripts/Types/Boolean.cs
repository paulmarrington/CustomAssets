// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

namespace CustomAsset {
  using UnityEngine;

  /// <inheritdoc />
  /// <summary>
  /// Boolean custom asset. Triggers event when changing from true to false or false to trye;
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#primitive-custom-assets">More...</a></remarks>
  [CreateAssetMenu(menuName = "Custom Assets/Boolean")]
  public sealed class Boolean : OfType<bool> {
    public override bool Equals(bool other) { return Value == other; }
  }
}