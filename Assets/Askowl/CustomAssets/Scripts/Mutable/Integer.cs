// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href="http://bit.ly/2QP2Kn9">Integer CustomAsset contains an int value. Add listeners to your own classes with Register(this)</a> <inheritdoc />
  [CreateAssetMenu(menuName = "Custom Assets/Mutable/Integer")]
  public class Integer : OfType<int> {
    /// <a href="http://bit.ly/2QP2Kn9"><see cref="OfType{T}.Instance{TC}"/></a>
    public static Integer Instance(string name) => Instance<Integer>(name);
  }
}