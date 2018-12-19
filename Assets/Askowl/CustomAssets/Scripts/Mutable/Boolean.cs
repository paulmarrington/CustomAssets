// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href="">Boolean custom asset. Triggers event when changing from true to false or false to true</a> //#TBD#//
  [CreateAssetMenu(menuName = "Custom Assets/Mutable/Boolean")]
  public class Boolean : OfType<bool> {
    /// <a href=""></a> //#TBD#//
    public static Boolean Instance(string name) => Instance<Boolean>(name);
  }
}