// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;

namespace CustomAsset.Mutable {
  /// <inheritdoc />
  /// <summary>
  /// Set or enum of strings - used to create custom asset
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#stringset">More...</a></remarks>
  [CreateAssetMenu(menuName = "Custom Assets/Mutable/String Set")]
  public class StringSet : Set<string> {
    public new static StringSet Instance(string name) {
      return Set<string>.Instance(name) as StringSet;
    }
  }
}