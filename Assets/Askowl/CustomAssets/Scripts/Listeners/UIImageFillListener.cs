using UnityEngine;
using UnityEngine.UI;

namespace CustomAsset {
  /// <summary>
  /// Drop into the same Game Object as an image component to update the fill amount
  /// whenever the Float custom asset changes.
  /// </summary>
  // ReSharper disable once InconsistentNaming
  [RequireComponent(typeof(Image))]
  public sealed class UIImageFillListener : FloatListener<Image> {
    protected override void Change(float value) { Component.fillAmount = value; }
  }
}