// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using Askowl;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href="http://bit.ly/2QQga27">Set or enum of strings - used to create custom asset</a> <inheritdoc />
  [CreateAssetMenu(menuName = "Custom Assets/Mutable/String Set")]
  public class StringSet : OfType<Set<string>> { }
}