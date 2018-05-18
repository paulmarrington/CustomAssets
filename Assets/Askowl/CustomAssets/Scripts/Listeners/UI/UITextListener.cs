// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using Decoupled;
using UnityEngine;

namespace CustomAsset {
  /// <inheritdoc />
  /// <summary>
  /// Drop into the same Game Object as a text component to update text content
  /// whenever the string custom asset changes.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#uitextlistener">More...</a></remarks>
  [RequireComponent(typeof(Textual))]
  // ReSharper disable once InconsistentNaming
  public sealed class UITextListener : StringListener<Textual> {
    /// <inheritdoc />
    protected override void Change(string value) { Component.text = value; }
  }
}