// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using Askowl;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href="">CustomAsset that contains a string. Events are triggered every time the string changes</a> //#TBD#// <inheritdoc />
  [CreateAssetMenu(menuName = "Custom Assets/Mutable/String"), Labels("Text")]
  public class String : OfType<string> {
    /// <a href=""></a> //#TBD#//
    public string Text {
      get => Value;
      set => Value = value;
    }

    /// <a href=""></a> //#TBD#//
    public static String Instance(string name) => Instance<String>(name);
  }
}