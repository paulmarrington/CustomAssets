// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;
using UnityEngine.UI;

namespace CustomAsset {
  /// <inheritdoc />
  /// <summary>
  /// Drop into the same Game Object as a text component to update text content
  /// whenever the string custom asset changes.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#uitextlistener">More...</a></remarks>
  [RequireComponent(typeof(Text))]
  // ReSharper disable once InconsistentNaming
  public sealed class UITextListener : StringListener<Text> {
    /// <inheritdoc />
    protected override void Change(string value) { Component.text = value; }
  }
}