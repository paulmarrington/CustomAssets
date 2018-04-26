﻿using UnityEngine;
using UnityEngine.UI;

namespace CustomAsset {
  /// <summary>
  /// Drop into the same Game Object as a text component to update text content
  /// whenever the string custom asset changes.
  /// </summary>
  // ReSharper disable once InconsistentNaming
  [RequireComponent(typeof(Text))]
  public sealed class UITextListener : ComponentListener<Text> {
    protected override void Change(string value) { Component.text = value; }
  }
}