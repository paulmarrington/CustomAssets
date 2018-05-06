using UnityEngine;
using UnityEngine.UI;

namespace CustomAsset {
  /// <inheritdoc />
  /// <summary>
  /// Drop into the same Game Object as a text component to update text content
  /// whenever the string custom asset changes.
  /// </summary>
  [RequireComponent(typeof(Text))]
  // ReSharper disable once InconsistentNaming
  public sealed class UITextListener : StringListener<Text> {
    protected override void Change(string value) { Component.text = value; }
  }
}