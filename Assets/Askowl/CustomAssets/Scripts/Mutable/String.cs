// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using Askowl;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href="http://bit.ly/2QP2Kn9">CustomAsset that contains a string. Events are triggered every time the string changes</a> <inheritdoc />
  [CreateAssetMenu(menuName = "Custom Assets/Mutable/String"), Labels("Text")]
  public class String : OfType<string> {
    /// <a href="http://bit.ly/2QP2Kn9">Set or retrieve text contents</a>
    public string Text { get => Value; set => Value = value; }

    /// <a href="http://bit.ly/2QP2Kn9"><see cref="OfType{T}.Instance{TC}"/></a>
    public static String Instance(string path) => Instance<String>(path);
  }
}