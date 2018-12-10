// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using Askowl;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <inheritdoc />
  /// <summary>
  /// .
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#uicanvasgroupalphalistener">More...</a></remarks>
  /// <a href="">Drop into the same Game Object as a canvas group component to update the transparency whenever the Float custom asset changes</a> //#TBD#// <inheritdoc />
  [RequireComponent(typeof(CanvasGroup))]
  public sealed class UiCanvasGroupAlphaListener : FloatListener<CanvasGroup> {
    /// <a href=""></a> //#TBD#// <inheritdoc />
    protected override void OnChange(float value) => Target.alpha = value;

    /// <a href=""></a> //#TBD#// <inheritdoc />
    protected override bool Equals(float value) => Compare.AlmostEqual(Target.alpha, value);
  }
}