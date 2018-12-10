// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;

namespace CustomAsset.Constant {
  /// <a href="">CustomAsset that contains a string. Events are triggered every time the string changes</a> //#TBD#// <inheritdoc />
  [CreateAssetMenu(menuName = "Custom Assets/Constant/String")]
  public sealed class String : OfType<string> { }
}