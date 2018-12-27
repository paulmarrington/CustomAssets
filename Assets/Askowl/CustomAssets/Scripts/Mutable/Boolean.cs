// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href="http://bit.ly/2QP2Kn9">Boolean custom asset. Triggers event when changing from true to false or false to true</a>
  [CreateAssetMenu(menuName = "Custom Assets/Mutable/Boolean")]
  public class Boolean : OfType<bool> {
    /// <a href="http://bit.ly/2QP2Kn9"></a>
    public static Boolean Instance(string name) => Instance<Boolean>(name);
  }
}