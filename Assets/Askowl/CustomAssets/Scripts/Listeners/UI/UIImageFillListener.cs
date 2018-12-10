// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using Askowl;
using UnityEngine;
using UnityEngine.UI;

namespace CustomAsset.Mutable {
  /// .
  /// <a href="">Drop into the same Game Object as an image component to update the fill amount whenever the Float custom asset changes</a> //#TBD#// <inheritdoc />
  [RequireComponent(typeof(Image))] public sealed class UiImageFillListener : FloatListener<Image> {
    /// <a href=""></a> //#TBD#// <inheritdoc />
    protected override void OnChange(float value) => Target.fillAmount = value;

    /// <a href=""></a> //#TBD#// <inheritdoc />
    protected override bool Equals(float value) => Compare.AlmostEqual(Target.fillAmount, value);
  }
}