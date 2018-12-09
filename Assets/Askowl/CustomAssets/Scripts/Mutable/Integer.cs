// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href="">Integer CustomAsset contains an int value. Add listeners to your own classes with Register(this)</a> //#TBD#// <inheritdoc />
  [CreateAssetMenu(menuName = "Custom Assets/Mutable/Integer")]
  public sealed class Integer : OfType<int> {
    /// <a href=""></a> //#TBD#//
    public static Integer Instance(string name) => Instance<Integer>(name);
  }
}