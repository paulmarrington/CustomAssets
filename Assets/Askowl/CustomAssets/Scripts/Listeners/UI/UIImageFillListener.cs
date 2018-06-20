// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;
using UnityEngine.UI;

namespace CustomAsset.Mutable {
  /// <inheritdoc />
  /// <summary>
  /// Drop into the same Game Object as an image component to update the fill amount
  /// whenever the Float custom asset changes.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#uiimagefilllistener">More...</a></remarks>
  [RequireComponent(typeof(Image))]
  public sealed class UIImageFillListener : FloatListener<Image> {
    /// <inheritdoc />
    protected override void OnChange(float value) { Target.fillAmount = value; }

    /// <inheritdoc />
    protected override bool Equals(float value) {
      return Compare.AlmostEqual(Target.fillAmount, value);
    }
  }
}