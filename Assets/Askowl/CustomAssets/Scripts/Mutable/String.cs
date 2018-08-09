// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using Askowl;

namespace CustomAsset.Mutable {
  using UnityEngine;

  /// <inheritdoc />
  /// <summary>
  /// CustomAsset that contains a string. Events are triggered every time the string changes
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#primitive-custom-assets">More...</a></remarks>
  [CreateAssetMenu(menuName = "Custom Assets/Mutable/String"), ValueName("Text")]
  public sealed class String : OfType<string> {
    public        string Text                  { get { return Value; } set { Value = value; } }
    public static String Instance(string name) { return Instance<String>(name); }
  }
}